using Coldsteel;
using System.Collections;

namespace LD46.Gameplay
{
	class Manager : Behavior
	{
		private Level level;

		public Manager(Level level)
		{
			this.level = level;
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
	}
}
