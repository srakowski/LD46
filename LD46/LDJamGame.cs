using Coldsteel;
using Coldsteel.DevTools;
using Microsoft.Xna.Framework;

namespace LD46
{
	class LDJamGame : Game
	{
		private GraphicsDeviceManager _graphics;
		private Engine _engine;

		public LDJamGame()
		{
			_graphics = new GraphicsDeviceManager(this)
			{
				PreferredBackBufferWidth = 1440,
				PreferredBackBufferHeight = 900,
			};
			_graphics.ApplyChanges();

			Window.AllowUserResizing = true;
			Content.RootDirectory = "Content";
			IsMouseVisible = true;

			_engine = new Engine(
				this,
				_graphics,
				new EngineConfig(
					new SceneFactory(),
					Controls.Create(),
					new Point(1280, 1024)
				)
			);

			_engine.UseDevTools();
		}

		protected override void Initialize()
		{
			base.Initialize();
			_engine.LoadScene(
				nameof(SceneFactory.MainMenuScene),
				new LDJamGameState()
			);
		}
	}
}
