using System;
using System.Collections;
using Coldsteel;
using Coldsteel.Audio;
using Coldsteel.UI;
using Coldsteel.UI.Elements;
using LD46.Shaders;
using Microsoft.Xna.Framework;

namespace LD46.MainMenu
{
	static class Factory
	{
		const string PLAY = "Play";
		const string SETTINGS = "Settings";
		const string EXIT = "Exit";

		public static Scene Create(LDJamGameState gameState)
		{
			var scene = new Scene();
			scene.AddAssetsFromDirectory(@"./Content");

			var fader = new Shaders.Fade();
			scene.SetShader(fader);

			var backgroundLayer = RenderingLayer.New("bg", 0)
				.AddToScene(scene);

			Entity.New
				.AddToScene(scene)
				.AddSprite("Texture2D/mainMenuBackground", backgroundLayer.Name);

			var options = new[]
			{
				CreateMenuOption(PLAY, 0),
				CreateMenuOption(SETTINGS, 1),
				CreateMenuOption(EXIT, 2)
			};

			var view = View.New.AddElement(
				Div.New
				.Configure(page =>
				{
					page.Dock = Dock.Fill;
				})
				.AddElement(
					Text.New.Configure(title =>
					{
						title.Value = "LDJAM46 GAME";
						title.Font = "Serif";
						title.Size = 48;
						title.Align = Align.Center;
						title.Anchor = Anchor.Center;
						title.Offset.Y = -200;
						title.Width = LDJamGame.GameWidth;
						title.Origin = Anchor.Center;
					})
				)
				.AddElement(options)
			);

			Entity.New
				.AddToScene(scene)
				.AddComponent(view)
				.AddComponent(new MenuController(options, gameState, fader));

			return scene;
		}

		private static Text CreateMenuOption(string value, int index)
		{
			return Text.New.Configure(opt =>
			{
				opt.Font = "Serif";
				opt.Value = value;
				opt.Size = 32;
				opt.Align = Align.Center;
				opt.Anchor = Anchor.Center;
				opt.Origin = Anchor.Center;
				opt.Width = LDJamGame.GameWidth;
				opt.Offset.Y = -80 + (80 * index);
				opt.Height = 80;
				opt.VerticalAlign = Align.Center;
			});
		}

		private class MenuController : Behavior
		{
			private int _idx = 0;
			private Text[] _options;
			private Controls _controls;
			private GameState _gameState;
			private bool _transitioning = false;
			private Fade _fade;

			public MenuController(Text[] options, GameState gameState, Fade fade)
			{ 
				_options = options;
				_gameState = gameState;
				_fade = fade;
			}

			protected override void Initialize()
			{
				UpdateOptions();
				_controls = Controls.Map(this);
			}

			protected override void Update()
			{
				if (_transitioning) return;
				if (_controls.Action.WasPushed())
				{
					if (_options[_idx].Value == PLAY)
						StartCoroutine(FadeOutThen(() => Engine.LoadScene(nameof(Gameplay), _gameState)));
					if (_options[_idx].Value == EXIT)
						Engine.ExitGame();
					return;
				}
				if (_controls.Up.WasPushed()) _idx--;
				if (_controls.Down.WasPushed()) _idx++;
				_idx = MathHelper.Clamp(_idx, 0, _options.Length - 1);
				UpdateOptions();
			}

			private IEnumerator FadeOutThen(Action doThis)
			{
				for (float f = 1f; f > 0f; f -= (0.002f * (float)GameTime.ElapsedGameTime.TotalMilliseconds))
				{
					_fade.Percent = f;
					if (_fade.Percent < 0f) _fade.Percent = 0f;
					yield return null;
				}
				doThis();
			}

			private void UpdateOptions()
			{
				ResetOptions(_options);
				Highlight(_options[_idx]);
			}

			private static void Highlight(Text option)
			{
				option.Size = 36;
				option.Color = Color.Red;
				option.Bold = true;
			}

			private static void ResetOptions(Text[] options)
			{
				foreach (var opt in options)
				{
					opt.Size = 32;
					opt.Color = Color.White;
					opt.Bold = false;
				}
			}
		}
	}
}
