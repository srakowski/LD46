using Coldsteel;
using Coldsteel.Animations;
using Microsoft.Xna.Framework;
using System.Linq;

namespace LD46.Gameplay
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

			new MissileSpawner(Settings.TurretFireFreq).AddToEntity(this);
		}

		public SpriteAnimator Animator { get; }

		public class MissileSpawner : Behavior
		{
			private int freq;
			private double timeSinceLastFire;

			public static float BasicSpeed => 0.2f;

			public MissileSpawner(int freq)
			{
				this.freq = freq;
			}

			protected override void Update()
			{
				timeSinceLastFire += this.GameTime.ElapsedGameTime.TotalMilliseconds;
				Spawn();
			}

			public void Spawn()
			{
				if (timeSinceLastFire < freq) return;

				var target = Scene.Entities.OfType<Slime>()
					.Select(c => new { E=c, D = Vector2.Distance(c.Position, Entity.Position)})
					.Where(c => c.D < Settings.TurretDistance)
					.OrderBy(c => c.D)
					.Select(c => c.E)
					.FirstOrDefault();

				if (target == null) return;

				var missile = new Missile(BasicSpeed);
				missile.Position = this.Entity.Position;
				missile.AddComponent(new SeekingMissileBehavior(missile, target, 8));
				
				if (missile == null) return;
				timeSinceLastFire = 0;
				Scene.AddEntity(missile);
			}
		}

		public class SeekingMissileBehavior : Behavior
		{
			private Missile missile;
			private Slime target;
			private int hitRadius;

			public SeekingMissileBehavior(Missile missile, Slime target, int hitRadius)
			{
				this.missile = missile;
				this.target = target;
				this.hitRadius = hitRadius;
			}

			protected override void Update()
			{
				if (target.Dead)
				{
					missile.Dead = true;
					return;
				}

				if (Vector2.Distance(target.Position, missile.Position) < hitRadius)
				{
					target.Hit(1);
					missile.Dead = true;
					return;
				}

				var direction = (target.Position - missile.Position);
				direction.Normalize();
				missile.Velocity = direction * missile.Speed;
				missile.Rotation = missile.Velocity.ToAngle();
				missile.Position += (missile.Velocity * (float)GameTime.ElapsedGameTime.TotalMilliseconds);
			}
		}
	}
}
