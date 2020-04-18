using Coldsteel;
using Coldsteel.UI;
using Coldsteel.UI.Elements;
using LD46.Gameplay.Entities;

namespace LD46.Gameplay
{
	static class Factory
	{
		public static Scene Create(PaceMakerGameState gameState)
		{
			var scene = new Scene();
			scene.AddAssetsFromDirectory(@"./Content");

			var map = new Map()
				.AddToScene(scene)
				.AddChild(
					Entity.New
						.SetPosition((Map.MapDimX * Tile.TileDim) * 0.5f, (Map.MapDimY * Tile.TileDim) * 0.5f)
						.AddCamera()
				);

			var player = new Player(map)
				.AddToScene(scene);

			return scene;
		}
	}
}
