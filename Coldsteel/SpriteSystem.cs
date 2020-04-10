// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Coldsteel
{
	internal class SpriteSystem : DrawableGameComponent
	{
		private readonly Dictionary<Scene, List<ISprite>> _spritesByScene = new Dictionary<Scene, List<ISprite>>();

		private readonly Dictionary<Scene, List<Camera>> _camerasByScene = new Dictionary<Scene, List<Camera>>();

		private readonly Engine _engine;

		private SpriteBatch _spriteBatch;

		private ViewportAdapter _vpa;

		private RenderTarget2D _renderTarget;

		public SpriteSystem(Game game, Engine engine) : base(game)
		{
			_engine = engine;
			game.Components.Add(this);
		}

		public override void Initialize()
		{
			base.Initialize();
			_vpa = new ViewportAdapter(Game.Window, Game.GraphicsDevice,
				_engine.Config.ScreenDim.X, _engine.Config.ScreenDim.Y);
			_spriteBatch = new SpriteBatch(Game.GraphicsDevice);
			_renderTarget = new RenderTarget2D(Game.GraphicsDevice,
				_engine.Config.ScreenDim.X, _engine.Config.ScreenDim.Y);
		}

		internal void AddSprite(Scene scene, ISprite sprite)
		{
			var spriteList = GetSpriteListForScene(scene);
			spriteList.Add(sprite);
		}

		internal void RemoveSprite(Scene scene, ISprite sprite)
		{
			var spriteList = GetSpriteListForScene(scene);
			spriteList.Remove(sprite);
		}

		private List<ISprite> GetSpriteListForScene(Scene scene)
		{
			return _spritesByScene.ContainsKey(scene)
				? _spritesByScene[scene]
				: (_spritesByScene[scene] = new List<ISprite>());
		}

		internal void AddCamera(Scene scene, Camera camera)
		{
			var cameraList = GetCameraListForScene(scene);
			cameraList.Add(camera);
		}

		internal void RemoveCamera(Scene scene, Camera camera)
		{
			var cameraList = GetCameraListForScene(scene);
			cameraList.Remove(camera);
		}

		private List<Camera> GetCameraListForScene(Scene scene)
		{
			return _camerasByScene.ContainsKey(scene)
				? _camerasByScene[scene]
				: (_camerasByScene[scene] = new List<Camera>());
		}

		public override void Draw(GameTime gameTime)
		{
			var scene = _engine.SceneManager.ActiveScene;
			if (scene == null) return;

			var sprites = GetSpriteListForScene(scene);
			var camera = GetCameraListForScene(scene).FirstOrDefault(c => c.Enabled);

			GraphicsDevice.SetRenderTarget(_renderTarget);
			Game.GraphicsDevice.Clear(camera?.BackgroundColor ?? Color.Black);
			foreach (var spriteLayer in scene.SpriteLayers.OrderBy(s => s.Depth))
			{
				var spritesThisLayer = sprites.Where(s => s.Enabled && s.SpriteLayerName == spriteLayer.Name);
				spriteLayer.Draw(_spriteBatch, camera, spritesThisLayer);
			}

			GraphicsDevice.SetRenderTarget(null);
			GraphicsDevice.Clear(Color.Black);

			_vpa.Reset();
			_spriteBatch.Begin(transformMatrix: _vpa.GetScaleMatrix());
			_spriteBatch.Draw(_renderTarget, Vector2.Zero, Color.White);
			_spriteBatch.End();
		}
	}
}
