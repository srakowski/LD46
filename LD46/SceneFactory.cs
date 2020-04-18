using Coldsteel;
using System;

namespace LD46
{
	class SceneFactory : ISceneFactory
	{
		public Scene Create(string sceneName, GameState gameState)
		{
			switch (sceneName)
			{
				case nameof(MainMenu): return MainMenu.Factory.Create(gameState as PaceMakerGameState);
				case nameof(Gameplay): return Gameplay.Factory.Create(gameState as PaceMakerGameState);
				default: throw new NotImplementedException("TODO");
			}
		}
	}
}

//var scene = new Scene();
//scene.AddAssetsFromDirectory(@"./Content");

//scene.AddSpriteLayers();

//Entity.New
//	.AddToScene(scene)
//	.AddSprite("Texture2D/dummy", SpriteLayers.Default);

//var sprite = new Sprite("Texture2D/dummy", SpriteLayers.Default, new Size(50, 50));
//var animator = new SpriteAnimator(sprite)
//	.AddSpriteAnimation("cycle", (0, 300), (1, 400), (2, 500), (3, 100))
//	.AddSpriteAnimation("to", (0, 100), (1, 100))
//	.AddSpriteAnimation("do", (2, 300), (3, 300));

//var emitter = Entity.New
//	.AddToScene(scene)
//	.SetPosition(500, 500);
//var ae = new AudioEmitter()
//	.AddToEntity(emitter);

//Entity.New
//	.AddToScene(scene)
//	.SetPosition(500, 500)
//	.AddComponent(sprite)
//	.AddComponent(animator)
//	.AddAudioListener()
//	.AddComponent(new RelayBehavior(onUpdate: rb =>
//	{
//		var left = rb.GetControl<ButtonControl>(Controls.PlayerLeft);
//		var right = rb.GetControl<ButtonControl>(Controls.PlayerRight);
//		var up = rb.GetControl<ButtonControl>(Controls.PlayerUp);
//		var down = rb.GetControl<ButtonControl>(Controls.PlayerDown);
//		var jump = rb.GetControl<ButtonControl>(Controls.PlayerJump);
//		if (jump.WasPushed()) ae.Play("SoundEffect/dummy");
//		if (left.IsDown()) rb.Entity.Position.X -= 10;
//		if (right.IsDown()) rb.Entity.Position.X += 10;
//		if (up.IsDown()) rb.Entity.Position.Y -= 10;
//		if (down.IsDown()) rb.Entity.Position.Y += 10;
//	}));

////int i = 0;

////var buttonText = Text.New
////	.Configure(text =>
////	{
////		text.Value = "Hello World!";
////		text.Size = 36;
////		text.Dock = Dock.Fill;
////		text.Align = Align.Center;
////		text.VerticalAlign = Align.Center;
////		text.Color = new Color(89, 94, 108);
////	});

////var image = Image.New
////	.Configure(img =>
////	{
////		img.Source = "./Content/Static/dummy.png";
////	});

////var gui = Entity.New
////	.AddComponent(new View().AddElement(
////		Div.New
////			.Configure(div =>
////			{
////				div.OnMouseClick += (s, ev) =>
////				{
////					i++;
////					buttonText.Value = $"Clicked {i}";
////					animator.Animate(
////						animator.CurrentAnimationName == "do"
////							? "to"
////							: "do"
////					);
////				};

////				div.Anchor = Anchor.Center;
////				div.BackgroundColor = Color.White;
////				div.BorderRadius = 8;
////				div.BorderColor = new Color(204, 207, 217);
////				div.Origin = Anchor.Center;
////				div.Height = 409;
////				div.Width = 630;
////				div.BorderWidth = 1;
////			})
////			.AddElement(
////				buttonText,
////				image
////			)
////	));

////scene.AddEntity(gui);

//return scene;