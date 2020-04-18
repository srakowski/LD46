// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Coldsteel.Audio;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coldsteel
{
	public class Entity
	{
		private bool _dead = false;

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

		public bool Dead
		{
			get => _dead || (_parent?.Dead ?? false);
			set => _dead = value;
		}

		public TComponent GetComponent<TComponent>() where TComponent : Component
		{
			return Components.OfType<TComponent>().FirstOrDefault();
		}

		public TComponent[] GetComponents<TComponent>() where TComponent : Component
		{
			return Components.OfType<TComponent>().ToArray();
		}

		public Entity AddComponent(Component component)
		{
			_components.Add(component);
			if (_engine != null)
				component.Activate(_engine, _scene, this);
			return this;
		}

		internal Entity AddChild(Entity entity)
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

		internal void Clean()
		{
			_children.ForEach(e => e.Clean());

			var deadEntities = _children.Where(e => e.Dead).ToArray();
			foreach (var deadEntity in deadEntities)
			{
				deadEntity.Deactivate();
				_children.Remove(deadEntity);
			}

			var deadComponents = _components.Where(c => c.Dead).ToArray();
			foreach (var deadComponent in deadComponents)
			{
				deadComponent.Deactivate();
				_components.Remove(deadComponent);
			}
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

		public static Entity New => new Entity();
	}

	public static class EntityExtensions
	{
		public static TEntity AddSprite<TEntity>(this TEntity entity, string assetName, string renderingLayer = null, Size? frameSize = null) where TEntity : Entity =>
			entity.AddComponent(new Sprite(assetName, renderingLayer, frameSize)) as TEntity;

		public static TEntity AddSprite<TEntity>(this TEntity entity, string assetName, Action<Sprite> configure) where TEntity : Entity
		{
			var sprite = new Sprite(assetName);
			configure?.Invoke(sprite);
			return entity.AddComponent(sprite) as TEntity;
		}

		public static TEntity AddTextSprite<TEntity>(this TEntity entity, string assetName, string text, string renderingLayer = null) where TEntity : Entity =>
			entity.AddComponent(new TextSprite(assetName, text, renderingLayer)) as TEntity;

		public static TEntity AddTextSprite<TEntity>(this TEntity entity, string assetName, string text, string renderingLayer = null, Action<TextSprite> configure = null) where TEntity : Entity
		{
			var sprite = new TextSprite(assetName, text, renderingLayer);
			configure?.Invoke(sprite);
			return entity.AddComponent(sprite) as TEntity;
		}

		public static TEntity AddCamera<TEntity>(this TEntity entity) where TEntity : Entity =>
			entity.AddComponent(new Camera()) as TEntity;

		public static TEntity AddCamera<TEntity>(this TEntity entity, Action<Camera> configure) where TEntity : Entity
		{
			var camera = new Camera();
			configure(camera);
			return entity.AddComponent(camera) as TEntity;
		}

		public static TEntity AddAudioListener<TEntity>(this TEntity entity) where TEntity : Entity
		{
			var al = new AudioListener();
			return entity.AddComponent(al) as TEntity;
		}

		public static TEntity AddAudioEmitter<TEntity>(this TEntity entity) where TEntity : Entity
		{
			var ae = new AudioEmitter();
			return entity.AddComponent(ae) as TEntity;
		}

		public static TEntity AddToScene<TEntity>(this TEntity entity, Scene scene) where TEntity : Entity
		{
			scene.AddEntity(entity);
			return entity;
		}

		public static TEntity AddChild<TEntity>(this TEntity entity, Entity child) where TEntity : Entity
		{
			return entity.AddChild(child) as TEntity;
		}
	}
}
