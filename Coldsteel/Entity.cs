﻿// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Coldsteel.Audio;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Coldsteel
{
	public class Entity
	{
		private readonly List<Component> _components = new List<Component>();

		private readonly List<Entity> _children = new List<Entity>();

		public IEnumerable<Entity> Children => _children;

		private Engine _engine;

		private Scene _scene;

		private Entity _parent;

		public Guid Id { get; } = Guid.NewGuid();

		public Vector2 Position;

		public Entity SetPosition(Vector2 position)
		{
			Position = position;
			return this;
		}

		public Entity SetPosition(float x, float y) => SetPosition(new Vector2(x, y));

		public Vector2 GlobalPosition => Vector2.Transform(Position, _parent?.TransformMatrix ?? Matrix.Identity);

		public float Rotation;

		public float GlobalRotation => Rotation + (_parent?.GlobalRotation ?? 0f);

		public float Scale = 1f;

		public float GlobalScale => Scale + (_parent?.GlobalScale - 1f ?? 0f);

		public Matrix TransformMatrix =>
			Matrix.Identity *
			Matrix.CreateRotationZ(GlobalRotation) *
			Matrix.CreateScale(GlobalScale) *
			Matrix.CreateTranslation(GlobalPosition.X, GlobalPosition.Y, 0f);

		public IEnumerable<Component> Components => _components;

		public Entity AddComponent(Component component)
		{
			_components.Add(component);
			if (_engine != null)
				component.Activate(_engine, _scene, this);
			return this;
		}

		public Entity AddChild(Entity entity)
		{
			_children.Add(entity);
			if (_engine != null)
				entity.Activate(_engine, _scene, this);
			return this;
		}

		internal void Activate(Engine engine, Scene scene, Entity parent)
		{
			_engine = engine;
			_scene = scene;
			_parent = parent;

			foreach (var entity in _children.ToArray())
				entity.Activate(engine, scene, this);

			foreach (var component in _components.ToArray())
				component.Activate(engine, scene, this);
		}

		internal void Deactivate()
		{
			foreach (var component in _components)
				component.Deactivate();

			foreach (var entity in _children.ToArray())
				entity.Deactivate();

			_parent = null;
			_scene = null;
			_engine = null;
		}

		public Entity AddSprite(string assetName, string spriteLayer, Size? frameSize = null) => AddComponent(new Sprite(assetName, spriteLayer, frameSize));
		public Entity AddSprite(string assetName, string spriteLayer, Size? frameSize, Action<Sprite> configure)
		{
			var sprite = new Sprite(assetName, spriteLayer, frameSize);
			configure(sprite);
			return AddComponent(sprite);
		}

		public Entity AddTextSprite(string assetName, string text, string spriteLayer) => AddComponent(new TextSprite(assetName, text, spriteLayer));
		public Entity AddTextSprite(string assetName, string text, string spriteLayer, Action<TextSprite> configure)
		{
			var sprite = new TextSprite(assetName, text, spriteLayer);
			configure(sprite);
			return AddComponent(sprite);
		}

		public Entity AddCamera() => AddComponent(new Camera());
		public Entity AddCamera(Action<Camera> configure)
		{
			var camera = new Camera();
			configure(camera);
			return AddComponent(camera);
		}

		public Entity AddAudioListener()
		{
			var al = new AudioListener();
			return AddComponent(al);
		}

		public Entity AddAudioEmitter()
		{
			var ae = new AudioEmitter();
			return AddComponent(ae);
		}

		public static Entity New => new Entity();
	}

	public static class EntityExtensions
	{
		public static TEntity AddToScene<TEntity>(this TEntity entity, Scene scene) where TEntity : Entity
		{
			scene.AddEntity(entity);
			return entity;
		}
	}
}
