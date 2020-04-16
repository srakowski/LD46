using Coldsteel;
using Microsoft.Xna.Framework;

namespace LD46
{
	class LDJamGame : Game
	{
		public const int GameWidth = 1280;
		public const int GameHeight = 1024;

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
					ControlsFactory.Create(),
					new Size(GameWidth, GameHeight)
				)
			);
		}

		protected override void Initialize()
		{
			base.Initialize();
			_engine.LoadScene(
				nameof(MainMenu),
				new LDJamGameState()
			);
		}
	}
}
