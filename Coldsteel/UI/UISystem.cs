// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace Coldsteel.UI
{
	internal class UISystem : SystemBase<View>
	{
		private SpriteBatch _spriteBatch;

		private Texture2D _texture;

		private GuiRenderer _guiRenderer;

		private byte[] _bytes;

		public UISystem(Game game, Engine engine) : base(game, engine)
		{
		}

		public override void Initialize()
		{
			base.Initialize();
			_spriteBatch = new SpriteBatch(Game.GraphicsDevice);
			_texture = new Texture2D(Game.GraphicsDevice, Engine.Config.ScreenDim.Width, Engine.Config.ScreenDim.Height, false, SurfaceFormat.Color);
			_guiRenderer = new GuiRenderer(_texture.Width, _texture.Height);
			_bytes = new byte[_texture.Width * _texture.Height * 4];
		}

		public override void Update(GameTime gameTime)
		{
			var views = ActiveComponents;
			if (views == null) return;

			var topView = views.FirstOrDefault();
			if (topView == null) return;

			var mousePos = Engine.RenderingSystem.PointToScreen(InputManager.Mouse.CurrentState.Position);

			topView.HandleMouseMovement(mousePos);

			if (InputManager.Mouse.CurrentState.LeftButton == ButtonState.Released &&
				InputManager.Mouse.PreviousState.LeftButton == ButtonState.Pressed)
			{
				topView.HandleMouseClick(mousePos);
			}

			var bounds = new Rectangle(Point.Zero, Engine.Config.ScreenDim);
			topView.UpdateLayout(bounds);
		}

		public void Render()
		{
			var views = ActiveComponents;
			if (views == null) return;
			if (!views.Any()) return;

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
