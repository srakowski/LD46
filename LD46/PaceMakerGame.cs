using Coldsteel;
using Microsoft.Xna.Framework;

namespace LD46
{
	class PaceMakerGame : Game
	{
		public const int GameWidth = 128;
		public const int GameHeight = 128;

		private GraphicsDeviceManager _graphics;
		private Engine _engine;

		public PaceMakerGame()
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
				nameof(Gameplay),
				new PaceMakerGameState()
			);
		}
	}
}
