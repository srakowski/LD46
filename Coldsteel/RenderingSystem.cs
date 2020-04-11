// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Coldsteel
{
	internal class RenderingSystem : DrawableGameComponent
	{
		private readonly Engine _engine;

		private ViewportAdapter _vpa;

		private SpriteBatch _spriteBatch;

		private RenderTarget2D _renderTarget;

		public RenderingSystem(Game game, Engine engine) : base(game)
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

		public override void Draw(GameTime gameTime)
		{
			GraphicsDevice.SetRenderTarget(_renderTarget);
			GraphicsDevice.Clear(Color.CornflowerBlue);

			_engine.SpriteSystem.Render();
			_engine.UISystem.Render();

			GraphicsDevice.SetRenderTarget(null);
			GraphicsDevice.Clear(Color.Black);

			_vpa.Reset();
			_spriteBatch.Begin(transformMatrix: _vpa.GetScaleMatrix());
			_spriteBatch.Draw(_renderTarget, Vector2.Zero, Color.White);
			_spriteBatch.End();
		}
	}
}
