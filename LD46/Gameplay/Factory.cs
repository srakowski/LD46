using Coldsteel;
using Coldsteel.UI;
using Coldsteel.UI.Elements;
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


			var level = new Level(24, 16)
				.AddToScene(scene);

			var c = new Camera();

			var goldText = Text.New.Configure(gt =>
			{
				gt.Value = "150";
				gt.Font = "Serif";
				gt.Size = 18;
				gt.Align = Align.Center;
				gt.Anchor = Anchor.Center;
				gt.Origin = Anchor.Center;
			});

			var v = new Manager(level, goldText);
			level.Manager = v;

			var view = new View();
			view.AddElement(goldText);

			//var view = new View(
			//			var view = View.New.AddElement(
			//	Div.New
			//	.Configure(page =>
			//	{
			//		page.Dock = Dock.Fill;
			//	})
			//	.AddElement(
			//		Text.New.Configure(title =>
			//		{
			//			title.Value = "LDJAM46 GAME";
			//			title.Font = "Serif";
			//			title.Size = 48;
			//			title.Align = Align.Center;
			//			title.Anchor = Anchor.Center;
			//			title.Offset.Y = -200;
			//			title.Width = TowerDefenseGame.GameWidth;
			//			title.Origin = Anchor.Center;
			//		})
			//	)
			//	.AddElement(options)
			//);

			var manager = Entity.New
				.AddToScene(scene)
				.AddComponent(v)
				.AddComponent(c)
				.AddComponent(new TurretPlacer(level, c))
				.SetPosition(level.CenterPoint);

			Entity.New.AddToScene(scene).AddComponent(view);

			return scene;
		}
	}
}
