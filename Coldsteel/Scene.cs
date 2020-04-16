// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Coldsteel
{
	public class Scene
	{
		private readonly List<Entity> _entities = new List<Entity>();

		private readonly List<RenderingLayer> _renderingLayers = new List<RenderingLayer>();

		private readonly List<Asset> _assets = new List<Asset>();

		private Engine _engine;

		private ContentManager _content;

		public Guid Id { get; } = Guid.NewGuid();

		public IEnumerable<Entity> Entities => _entities;

		public Scene AddEntity(Entity entity)
		{
			_entities.Add(entity);
			if (_engine != null)
				entity.Activate(_engine, this, null);
			return this;
		}

		public IEnumerable<Asset> Assets => _assets;

		public Scene AddAsset(Asset asset)
		{
			_assets.Add(asset);
			if (_content != null)
				asset.Load(_content);
			return this;
		}

		public Scene AddAssetsFromDirectory(string rootDirectory)
		{
			AddAssetsFromDirectory<Texture2D>(rootDirectory);
			AddAssetsFromDirectory<SpriteFont>(rootDirectory);
			AddAssetsFromDirectory<SoundEffect>(rootDirectory);
			AddAssetsFromDirectory<Song>(rootDirectory);
			AddAssetsFromDirectory<Effect>(rootDirectory);
			return this;
		}

		private Scene AddAssetsFromDirectory<T>(string rootDirectory)
		{
			var folder = typeof(T).Name;
			var path = Path.Combine(rootDirectory, folder);
			if (!Directory.Exists(path)) return this;
			var files = Directory.GetFiles(path, "*.xnb");
			foreach (var file in files)
			{
				var name = Path.GetFileNameWithoutExtension(file);
				AddAsset(new Asset<T>($"{folder}/{name}"));
			}
			return this;
		}

		public IEnumerable<RenderingLayer> RenderingLayers => _renderingLayers;

		public Scene AddRenderingLayer(RenderingLayer renderingLayer)
		{
			_renderingLayers.Add(renderingLayer);
			return this;
		}

		internal void Activate(Engine engine)
		{
			_engine = engine;

			LoadContent();

			foreach (var entity in _entities.ToArray())
				entity.Activate(engine, this, null);
		}

		internal void Clean()
		{
			_entities.ForEach(e => e.Clean());
			var deadEntities = _entities.Where(e => e.Dead).ToArray();
			foreach (var deadEntity in deadEntities)
			{
				deadEntity.Deactivate();
				_entities.Remove(deadEntity);
			}
		}

		internal void Deactivate()
		{
			foreach (var entity in _entities)
				entity.Deactivate();

			foreach (var contentDependency in _assets)
				contentDependency.Unload();

			_content.Unload();
			_content = null;
			_engine = null;
		}

		private void LoadContent()
		{
			_content = new ContentManager(_engine.Game.Services, _engine.Game.Content.RootDirectory);
			foreach (var asset in _assets)
				asset.Load(_content);
		}

		/// <summary>
		/// Used for hot reload of content.
		/// </summary>
		internal void ReloadContent()
		{
			if (_content == null) return;
			var oldContent = _content;
			LoadContent();
			oldContent.Unload();
		}

		public void Load(string sceneName, GameState gameState)
		{
			if (_engine == null) return;
			_engine.LoadScene(sceneName, gameState);
		}

		public Scene AddTexture2D(string name) => AddAsset(new Asset<Texture2D>(name));
		public Scene AddSpriteFont(string name) => AddAsset(new Asset<SpriteFont>(name));
		public Scene AddSoundEffect(string name) => AddAsset(new Asset<SoundEffect>(name));
		public Scene AddSong(string name) => AddAsset(new Asset<Song>(name));

		public Scene AddRenderingLayer(string name, int depth) => AddRenderingLayer(
			new RenderingLayer(name)
			{
				Depth = depth
			}
		);

		internal Component FindComponentById(Guid id)
		{
			return FindComponentById(_entities, id);
		}

		private static Component FindComponentById(IEnumerable<Entity> entities, Guid id)
		{
			if (!entities.Any()) return null;

			var c = entities
				.SelectMany(e => e.Components)
				.FirstOrDefault(e => e.Id == id);

			if (c != null) return c;

			return FindComponentById(entities.SelectMany(e => e.Children), id);
		}
	}
}