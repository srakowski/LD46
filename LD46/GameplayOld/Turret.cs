using Coldsteel;
using Coldsteel.Animations;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace LD46.GameplayOld
{
	class Turret : Entity
	{
		public Turret()
		{
			var sprite = new Sprite(
				Assets.Texture2D.tower,
				frameSize: new Size(12, 12)
			)
			{
				FrameIndex = 0,
				Origin = new Vector2(6, 6),
			}
			.AddToEntity(this);

			var frames = Enumerable.Range(1, 5)
				.Select(i => new Frame(i, 60))
				.ToArray();

			Animator = new SpriteAnimator(sprite)
				.AddSpriteAnimation("idle", (0, 0))
				.AddSpriteAnimation("shimmer", frames)
				.AddToEntity(this);

			Animator.Animate("shimmer");

			var ms = new MissileSpawner(500).AddToEntity(this);
			var tb = new TurrentBehavior(ms).AddToEntity(this);
		}

		public SpriteAnimator Animator { get; }

		public class TurrentBehavior : Behavior
		{
			protected override void Update()
			{
				if (spawnMissile == null) spawnMissile = SpawnMissile;
				ms.Spawn(spawnMissile);
			}

			private Func<Missile> spawnMissile;
			private MissileSpawner ms;

			public TurrentBehavior(MissileSpawner ms)
			{
				this.ms = ms;
			}

			private Missile SpawnMissile()
			{
				var target = Scene.Entities.OfType<Slime>()
					.OrderBy(m => Vector2.Distance(m.Position, Entity.Position))
					.FirstOrDefault();
				if (target == null) return null;
				return new Missile(Entity.Position, target, Missile.BasicSpeed, 8);
			}
		}
	}
}
