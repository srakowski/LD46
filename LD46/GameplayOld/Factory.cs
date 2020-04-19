using Coldsteel;
using Microsoft.Xna.Framework.Graphics;

namespace LD46.GameplayOld
{
	static class Factory
	{
		public const string MapBelowPlayer = "mapBelowPlayer";
		public const string MapAbovePlayer = "mapAbovePlayer";

		public static Scene Create(TowerDefenseGameState gameState)
		{
			var scene = new Scene();
			scene.AddAssetsFromDirectory(@"./Content");

			scene.ClearRenderingLayers();
			scene.AddRenderingLayer(new RenderingLayer(null, 0)
			{
				SamplerState = SamplerState.PointClamp,
			});

			scene.AddRenderingLayer(new RenderingLayer(MapBelowPlayer, -10) { SamplerState = SamplerState.PointClamp });
			scene.AddRenderingLayer(new RenderingLayer(MapAbovePlayer, 10) { SamplerState = SamplerState.PointClamp });

			var map = new Map()
				.AddToScene(scene);

			var camera = new Camera();

			var player = new Player(camera, map)
				.AddToScene(scene);

			var monster = new Slime()
				.SetPosition(128, 128)
				.AddToScene(scene);

			new Slime()
				.SetPosition(256, 265)
				.AddToScene(scene);

			new Turret()
				.SetPosition(30, 30)
				.AddToScene(scene);

			new Entity()
				.AddComponent(new GameplayController())
				.AddComponent(camera)
				.AddAudioListener()
				.AddToScene(scene);

			return scene;
		}
	}
}
