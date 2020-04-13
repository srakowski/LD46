using Coldsteel;
using Coldsteel.Animations;
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
				case nameof(DummySceneNotToBeUsedForActualGame): return DummySceneNotToBeUsedForActualGame(gameState as LDJamGameState);
				case nameof(GameplayScene): return GameplayScene(gameState as LDJamGameState);
				default: throw new NotImplementedException("TODO");
			}
		}

		public Scene DummySceneNotToBeUsedForActualGame(LDJamGameState gameState)
		{
			var scene = new Scene();
			scene.AddAssetsFromDirectory(@"./Content");

			scene.AddSpriteLayers();

			Entity.New
				.AddToScene(scene)
				.AddSprite("Texture2D/dummy", SpriteLayers.Default);

			var sprite = new Sprite("Texture2D/dummy", SpriteLayers.Default, new Size(50, 50));
			var animator = new SpriteAnimator(sprite)
				.AddSpriteAnimation("cycle", (0, 300), (1, 400), (2, 500), (3, 100))
				.AddSpriteAnimation("to", (0, 200), (1, 200))
				.AddSpriteAnimation("do", (2, 300), (3, 300));

			Entity.New
				.AddToScene(scene)
				.SetPosition(200, 0)
				.AddComponent(sprite)
				.AddComponent(animator);

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
				.AddComponent(new View().AddElement(
					Div.New
						.Configure(div =>
						{
							div.OnMouseClick += (s, ev) =>
							{
								i++;
								buttonText.Value = $"Clicked {i}";
								animator.Animate(
									animator.CurrentAnimationName == "do"
										? "to"
										: "do"
								);
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
						.AddElement(
							buttonText,
							image
						)
				));

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
