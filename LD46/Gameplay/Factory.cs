using Coldsteel;
using Coldsteel.UI;
using Coldsteel.UI.Elements;
using LD46.Gameplay;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

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

			scene.AddRenderingLayer(new RenderingLayer("hud", 3)
			{
				SamplerState = SamplerState.PointClamp,
				SpriteSortMode = SpriteSortMode.FrontToBack,
			});


			var level = new Level(24, 16)
				.AddToScene(scene);

			var c = new Camera();

			var goldText = Text.New.Configure(gt =>
			{
				gt.Width = 200;
				gt.Value = "Gold: ";
				gt.Font = "Serif";
				gt.Size = 14;
				gt.Align = Align.Near;
				gt.Anchor = Anchor.TopLeft;
				gt.Origin = Anchor.TopLeft;
			});


			var purityText = Text.New.Configure(gt =>
			{
				gt.Width = 200;
				gt.Value = "Purity: ";
				gt.Font = "Serif";
				gt.Size = 14;
				gt.Align = Align.Near;
				gt.Anchor = Anchor.TopLeft;
				gt.Origin = Anchor.TopLeft;
				gt.Offset = new Microsoft.Xna.Framework.Point(0, 30);
			});

			var v = new Manager(level, goldText, purityText);
			level.Manager = v;

			var view = new View();
			view.AddElement(Div.New
				.Configure(d =>
				{
					d.Height = 100;
					d.Anchor = Anchor.BottomLeft;
					d.Origin = Anchor.BottomLeft;
				})
				.AddElement(
					goldText,
					purityText
			));

			var pickers = new Dictionary<TurretyType, TurretPicker>()
			{
				{TurretyType.BlueTurret,  new TurretPicker(TurretyType.BlueTurret) },
				{TurretyType.Green,  new TurretPicker(TurretyType.Green) },
				{TurretyType.Red,  new TurretPicker(TurretyType.Red) },
				{TurretyType.Dark,  new TurretPicker(TurretyType.Dark) },
			};

			var manager = Entity.New
				.AddToScene(scene)
				.AddComponent(v)
				.AddComponent(c)
				.AddComponent(new TurretPlacer(level, c, pickers))
				.SetPosition(level.CenterPoint);


			foreach (var picker in pickers.Values)
			{
				manager.AddChild(picker);
			}

			Entity.New.AddToScene(scene)
				.AddComponent(view);

			return scene;
		}
	}
}
