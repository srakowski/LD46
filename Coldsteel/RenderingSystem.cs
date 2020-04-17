// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Coldsteel
{
	internal class RenderingSystem : DrawableGameComponent
	{
		private readonly Dictionary<Scene, List<Camera>> _camerasByScene = new Dictionary<Scene, List<Camera>>();

		private readonly Dictionary<Scene, List<IRenderer>> _renderersByScene = new Dictionary<Scene, List<IRenderer>>();

		private readonly Engine _engine;

		private ViewportAdapter _vpa;

		private SpriteBatch _spriteBatch;

		private RenderTarget2D _renderTarget;

		public RenderingSystem(Game game, Engine engine) : base(game)
		{
			_engine = engine;
			game.Components.Add(this);
		}

		public Point PointToScreen(Point point)
		{
			return _vpa.PointToScreen(point);
		}

		public override void Initialize()
		{
			base.Initialize();
			_vpa = new ViewportAdapter(Game.Window, Game.GraphicsDevice,
				_engine.Config.ScreenDim.Width, _engine.Config.ScreenDim.Height);
			_spriteBatch = new SpriteBatch(Game.GraphicsDevice);
			_renderTarget = new RenderTarget2D(Game.GraphicsDevice,
				_engine.Config.ScreenDim.Width, _engine.Config.ScreenDim.Height);
		}

		public override void Draw(GameTime gameTime)
		{
			GraphicsDevice.SetRenderTarget(_renderTarget);
			GraphicsDevice.Clear(Color.Black);

			var scene = _engine.SceneManager.ActiveScene;
			if (scene != null)
			{
				var renderers = GetRendererListForScene(scene);
				var camera = GetCameraListForScene(scene).FirstOrDefault(c => c.Enabled);

				foreach (var renderingLayer in scene.RenderingLayers.OrderBy(s => s.Depth))
				{
					var renderersThisLayer = renderers
						.Concat(_engine.ParticleSystem.Particles.Cast<IRenderer>())
						.Where(s => s.Enabled && s.RenderingLayerName == renderingLayer.Name);
						
					renderingLayer.Draw(_spriteBatch, camera, renderersThisLayer);
				}
			}

			_engine.UISystem.Render();

			GraphicsDevice.SetRenderTarget(null);
			GraphicsDevice.Clear(Color.Black);

			_vpa.Reset();
			var shader = scene.Shader;
			_spriteBatch.Begin(effect: shader?.Effect, transformMatrix: _vpa.GetScaleMatrix());
			shader?.ApplyParameters();
			_spriteBatch.Draw(_renderTarget, Vector2.Zero, Color.White);
			_spriteBatch.End();
		}

		internal void AddRenderer(Scene scene, IRenderer sprite)
		{
			var spriteList = GetRendererListForScene(scene);
			spriteList.Add(sprite);
		}

		internal void RemoveRenderer(Scene scene, IRenderer sprite)
		{
			var spriteList = GetRendererListForScene(scene);
			spriteList.Remove(sprite);
		}

		private List<IRenderer> GetRendererListForScene(Scene scene)
		{
			return _renderersByScene.ContainsKey(scene)
				? _renderersByScene[scene]
				: (_renderersByScene[scene] = new List<IRenderer>());
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
	}
}
