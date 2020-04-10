using Coldsteel;
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

			Window.AllowUserResizing = true;
			Content.RootDirectory = "Content";
			IsMouseVisible = true;

			_engine = new Engine(this, new EngineConfig(
				new SceneFactory(),
				Controls.Create()
			));

		}
	}
}
