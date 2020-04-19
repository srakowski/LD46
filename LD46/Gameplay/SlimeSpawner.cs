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

		public void SpawnWave(int numberOfSlimes) => behavior.SpawnWave(numberOfSlimes);

		public class SpawnBehavior : Behavior
		{
			private Level level;

			public SpawnBehavior(Level level)
			{
				this.level = level;
			}

			public void SpawnWave(int numberOfSlimes)
			{
				StartCoroutine(BeginSpawnWave(numberOfSlimes));
			}

			private IEnumerator BeginSpawnWave(int numberOfSlimes)
			{
				for (int i = 0; i < numberOfSlimes; i++)
				{
					SpawnSlime(1.0f / ((float)i + 1));
					yield return Wait.Duration(800);
				}
			}

			private void SpawnSlime(float depth)
			{
				new Slime(level, depth)
					.SetPosition(this.Entity.Position)
					.AddToScene(Scene);
			}
		}
	}
}
