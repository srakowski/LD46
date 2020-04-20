using Coldsteel;
using Coldsteel.UI.Elements;
using System;
using System.Collections;

namespace LD46.Gameplay
{
	class Manager : Behavior
	{
		private Level level;
		public int Gold = Settings.StartingGold;
		public Text goldText;
		public Text purityText;
		public int Purity = Settings.StartingPurity;

		public Manager(Level level, Coldsteel.UI.Elements.Text goldText, Coldsteel.UI.Elements.Text purityText)
		{
			this.Gold = Settings.StartingGold;
			this.level = level;
			this.goldText = goldText;
			this.purityText = purityText;
		}

		protected override void Start()
		{
			PlaySong(Assets.Song.ninety, loop: true);
			StartCoroutine(RunGame());
		}

		private IEnumerator RunGame()
		{
			yield return Wait.Duration(30000);
			for (int i = 0; i < 100; i++)
			{
				level.SlimeSpawner.SpawnWave(20 * (i + 1), i);
				yield return Wait.Duration(30000);
			}
		}

		protected override void Update()
		{
			goldText.Value = $"Gold: {Gold}";
			purityText.Value = $"Purity: {Purity}";
			if (this.Purity == 0)
			{
				StopSong();
				Engine.LoadScene(nameof(GameOver), null);
			}
		}

		internal void AddGold(int gold)
		{
			Gold += gold;
		}

		internal void Buy(int gold)
		{
			Gold -= gold;
		}

		internal void ReducePurity(int v)
		{
			Purity -= v;
		}
	}
}
