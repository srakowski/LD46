using Coldsteel;
using Coldsteel.Controls;
using Coldsteel.UI;
using Coldsteel.UI.Elements;
using Microsoft.Xna.Framework;
using System;

namespace LD46
{
	class SceneFactory : ISceneFactory
	{
		public Scene Create(string sceneName, GameState gameState)
		{
			switch (sceneName)
			{
				case nameof(MainMenuScene): return MainMenuScene(gameState as LDJamGameState);
				case nameof(GameplayScene): return GameplayScene(gameState as LDJamGameState);
				default: throw new NotImplementedException("TODO");
			}
		}

		public Scene MainMenuScene(LDJamGameState gameState)
		{
			var scene = new Scene();

			// TODO: add assets

			scene.AddSpriteLayers();

			// TODO: add entities

			int i = 0;

			var buttonText = Text.New
				.Configure(text =>
				{
					text.Value = "Hello World!";
					text.Size = 36;
					text.Dock = Dock.Fill;
					text.Align = Align.Center;
					text.VerticalAlign = Align.Center;
					text.Color = new Color(89, 94, 108);
				});

			var image = Image.New
				.Configure(img =>
				{
					img.Source = "./Content/Static/dummy.png";
				});

			var gui = Entity.New
				.AddComponent(new View
				{
					Div.New
						.Configure(div =>
						{
							div.OnMouseClick += (s, e) =>
							{
								i++;
								buttonText.Value = $"Clicked {i}";
							};

							div.Anchor = Anchor.Center;
							div.BackgroundColor = Color.White;
							div.BorderRadius = 8;
							div.BorderColor = new Color(204, 207, 217);
							div.Origin = Anchor.Center;
							div.Height = 409;
							div.Width = 630;
							div.BorderWidth = 1;
						})
						.Add(
							buttonText,
							image
						),
				});

			scene.AddEntity(gui);

			return scene;
		}

		public Scene GameplayScene(LDJamGameState gameState)
		{
			var scene = new Scene();

			// TODO: add assets

			scene.AddSpriteLayers();

			// TODO: add entities

			return scene;
		}
	}
}
