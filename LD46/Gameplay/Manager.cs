using Coldsteel;
using Coldsteel.UI.Elements;
using System.Collections;

namespace LD46.Gameplay
{
	class Manager : Behavior
	{
		private Level level;
		public int Gold;
		public Text goldText;

		public Manager(Level level, Coldsteel.UI.Elements.Text goldText)
		{
			this.level = level;
			this.goldText = goldText;
		}

		protected override void Start()
		{
			PlaySong(Assets.Song.ninety, loop: true);
			StartCoroutine(RunGame());
		}

		private IEnumerator RunGame()
		{
			for (int i = 0; i < 10; i++)
			{
				level.SlimeSpawner.SpawnWave(6 * (i + 1), i);
				yield return Wait.Duration(30000);
			}
		}

		protected override void Update()
		{
			goldText.Value = Gold.ToString();
		}

		internal void AddGold(int gold)
		{
			Gold += gold;
		}
	}
}
