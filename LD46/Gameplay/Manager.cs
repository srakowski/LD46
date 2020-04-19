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
			for (int i = 0; i < 1; i++)
			{
				yield return Wait.Duration(3000);
				level.SlimeSpawner.SpawnWave(10);
			}
		}
	}
}
