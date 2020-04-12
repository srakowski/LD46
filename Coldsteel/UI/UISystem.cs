// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace Coldsteel.UI
{
	internal class UISystem : GameComponent
	{
		private readonly Dictionary<Scene, List<View>> _viewsByScene = new Dictionary<Scene, List<View>>();

		private readonly Engine _engine;

		private SpriteBatch _spriteBatch;

		private Texture2D _texture;

		private GuiRenderer _guiRenderer;

		private byte[] _bytes;

		public UISystem(Game game, Engine engine) : base(game)
		{
			_engine = engine;
			game.Components.Add(this);
		}

		public override void Initialize()
		{
			base.Initialize();
			_spriteBatch = new SpriteBatch(Game.GraphicsDevice);
			_texture = new Texture2D(Game.GraphicsDevice, _engine.Config.ScreenDim.X, _engine.Config.ScreenDim.Y, false, SurfaceFormat.Color);
			_guiRenderer = new GuiRenderer(_texture.Width, _texture.Height);
			_bytes = new byte[_texture.Width * _texture.Height * 4];
		}

		internal void AddView(Scene scene, View view)
		{
			var list = GetViewsForScene(scene);
			list.Add(view);
			view.UpdateLayout(new Rectangle(Point.Zero, _engine.Config.ScreenDim));
		}

		internal void RemoveView(Scene scene, View view)
		{
			var list = GetViewsForScene(scene);
			list.Remove(view);
		}

		private List<View> GetViewsForScene(Scene scene)
		{
			return _viewsByScene.ContainsKey(scene)
				? _viewsByScene[scene]
				: (_viewsByScene[scene] = new List<View>());
		}

		public override void Update(GameTime gameTime)
		{
			var scene = _engine.SceneManager.ActiveScene;
			if (scene == null) return;
			var views = GetViewsForScene(scene);
			var topView = views.FirstOrDefault();
			if (topView == null) return;

			var mousePos = _engine.RenderingSystem.PointToScreen(InputManager.Mouse.CurrentState.Position);

			topView.HandleMouseMovement(mousePos);

			if (InputManager.Mouse.CurrentState.LeftButton == ButtonState.Released &&
				InputManager.Mouse.PreviousState.LeftButton == ButtonState.Pressed)
			{
				topView.HandleMouseClick(mousePos);
			}

			var bounds = new Rectangle(Point.Zero, _engine.Config.ScreenDim);
			topView.UpdateLayout(bounds);
		}

		public void Render()
		{
			var scene = _engine.SceneManager.ActiveScene;
			if (scene == null) return;

			var views = GetViewsForScene(scene);

			_spriteBatch.Begin(blendState: BlendState.NonPremultiplied);
			foreach (var view in views)
			{
				view.Render(_guiRenderer, _bytes);
				_texture.SetData(_bytes, 0, _bytes.Length);
				_spriteBatch.Draw(_texture, Vector2.Zero, Color.White);
			}
			_spriteBatch.End();
		}
	}
}
