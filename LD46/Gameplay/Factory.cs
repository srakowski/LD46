using Coldsteel;
using Microsoft.Xna.Framework.Graphics;

namespace LD46.Gameplay
{
	static class Factory
	{
		public static Scene Create(PaceMakerGameState gameState)
		{
			var scene = new Scene();
			scene.AddAssetsFromDirectory(@"./Content");

			scene.ClearRenderingLayers();
			scene.AddRenderingLayer(new RenderingLayer(null, 0)
			{
				SamplerState = SamplerState.PointClamp,
			});


			//var map = new Map()
			//	.AddToScene(scene)
			//	.AddChild(
			//		Entity.New
			//			.SetPosition((Map.MapDimX * Tile.TileDim) * 0.5f, (Map.MapDimY * Tile.TileDim) * 0.5f)
			//			.AddCamera()
			//	);

			//var player = new Player(map)
			//	.AddToScene(scene);

			new Entity()
				.AddSprite(Assets.Texture2D.example)
				.AddToScene(scene);


			var player = new Player()
				.AddToScene(scene);


			var monster = new Monster()
				.SetPosition(128, 128)
				.AddToScene(scene);

			new MagicMissile()
				.AddToScene(scene);

			new Entity()
				.AddComponent(new GameplayController())
				.AddToScene(scene);

			return scene;
		}
	}
}
