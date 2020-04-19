using Coldsteel;
using LD46.Gameplay;
using Microsoft.Xna.Framework.Graphics;

namespace LD46.Gameplay
{
	static class Factory
	{
		public static Scene Create(TowerDefenseGameState gameState)
		{
			var scene = new Scene();
			scene.AddAssetsFromDirectory(@"./Content");

			scene.ClearRenderingLayers();
			scene.AddRenderingLayer(new RenderingLayer(null, 0)
			{
				SamplerState = SamplerState.PointClamp,
			});

			scene.AddRenderingLayer(new RenderingLayer("slimes", 1)
			{
				SamplerState = SamplerState.PointClamp,
				SpriteSortMode = SpriteSortMode.FrontToBack,
			});

			scene.AddRenderingLayer(new RenderingLayer("missiles", 2)
			{
				SamplerState = SamplerState.PointClamp,
				SpriteSortMode = SpriteSortMode.FrontToBack,
			});


			var level = new Level(16, 12)
				.AddToScene(scene);

			var c = new Camera();

			var manager = Entity.New
				.AddToScene(scene)
				.AddComponent(new Manager(level))
				.AddComponent(c)
				.AddComponent(new TurretPlacer(level, c))
				.SetPosition(level.CenterPoint);

			return scene;
		}
	}
}
