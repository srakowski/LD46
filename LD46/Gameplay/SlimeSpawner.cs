using Coldsteel;
using System;
using System.Collections;

namespace LD46.Gameplay
{
	class SlimeSpawner : Entity
	{
		private Level level;
		private SpawnBehavior behavior;

		public SlimeSpawner(Level level)
		{
			this.level = level;
			new Sprite(Assets.Texture2D.spawn)
			{
				Origin = new Microsoft.Xna.Framework.Vector2(24, 40),
			}.AddToEntity(this);
			this.behavior = new SpawnBehavior(level).AddToEntity(this);
		}

		public void SpawnWave(int numberOfSlimes, int level) => behavior.SpawnWave(numberOfSlimes, level);

		public class SpawnBehavior : Behavior
		{
			private Level level;

			public SpawnBehavior(Level level)
			{
				this.level = level;
			}

			public void SpawnWave(int numberOfSlimes, int rank)
			{
				StartCoroutine(BeginSpawnWave(numberOfSlimes, rank));
			}

			private IEnumerator BeginSpawnWave(int numberOfSlimes, int rank)
			{
				for (int i = 0; i < numberOfSlimes; i++)
				{
					SpawnSlime(1.0f / ((float)i + 1), rank);
					yield return Wait.Duration(600);
				}
			}

			private void SpawnSlime(float depth, int rank)
			{
				new Slime(level, depth, rank)
					.SetPosition(this.Entity.Position)
					.AddToScene(Scene);
			}
		}
	}
}
