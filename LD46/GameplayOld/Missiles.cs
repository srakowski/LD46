using Coldsteel;
using Coldsteel.Animations;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace LD46.GameplayOld
{
	public class MissileSpawner : Behavior
	{
		private int freq;
		private double timeSinceLastFire;

		public MissileSpawner(int freq)
		{
			this.freq = freq;
		}

		protected override void Update()
		{
			timeSinceLastFire += this.GameTime.ElapsedGameTime.TotalMilliseconds;
		}

		public void Spawn(Func<Missile> missileFactory)
		{
			if (timeSinceLastFire < freq) return;
			var missile = missileFactory.Invoke();
			if (missile == null) return;
			timeSinceLastFire = 0;
			Scene.AddEntity(missile);
		}
	}

	public class Missile : Entity
	{
		public static float BasicSpeed => 0.2f;

		public Missile()
		{
			var sprite = new Sprite(
				Assets.Texture2D.magicMissile,
				frameSize: new Size(8, 16)
			)
			{
				FrameIndex = 0,
				Origin = new Vector2(4, 8),
			}
			.AddToEntity(this);

			var frames = Enumerable.Range(0, 6)
				.Select(i => new Frame(i, 60))
				.ToArray();

			Animator = new SpriteAnimator(sprite)
				.AddSpriteAnimation("fire", frames)
				.AddToEntity(this);

			Animator.Animate("fire");
		}

		public SeekingMissileBehavior Behavior { get; }

		public SpriteAnimator Animator { get; }

		public Missile(Vector2 sourcePosition, Entity target, float speed, int hitRadius) : this()
		{
			Position = sourcePosition;
			var direction = target.Position - sourcePosition;
			direction.Normalize();
			Speed = speed;
			Velocity = direction * speed;
			Rotation = Velocity.ToAngle();
			Behavior = new SeekingMissileBehavior(this, target, hitRadius)
				.AddToEntity(this);
		}

		public Vector2 Velocity = Vector2.Zero;
		public float Speed = 0.1f;
	}

	public class SeekingMissileBehavior : Behavior
	{
		private Missile missile;
		private Entity target;
		private int hitRadius;

		public SeekingMissileBehavior(Missile missile, Entity target, int hitRadius)
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




	//public class MissileSpawner : Behavior
	//{
	//	private MissileSpawningStrategy _spawnerStrategy;
	//	private Func<Vector2, Vector2, Entity> _projectileFactory;

	//	public MissileSpawner(MissileSpawningStrategy spawnerStrategy, Func<Vector2, Vector2, Entity> projectileFactory)
	//	{
	//		_spawnerStrategy = spawnerStrategy;
	//		_projectileFactory = projectileFactory;
	//	}

	//	protected override void Initialize()
	//	{
	//		StartCoroutine(_spawnerStrategy.Begin(this));
	//	}

	//	protected void Spawn(Vector2 direction)
	//	{
	//		AudioPlayer.PlaySfx("spawn");
	//		_sceneManager.ActiveScene.AddEntity(_projectileFactory.Invoke(Entity.Transform.Position, direction));
	//	}
	//}

	//public class Missile : Entity
	//{
	//	public Missile()
	//	{
	//	}
	//}

	//public class MissileController : Behavior
	//{
	//	public Vector2 _velocity;
	//	private int _dmg;

	//	public MissileController(Vector2 direction, float speed, int dmg)
	//	{
	//		_velocity = direction;
	//		_velocity.Normalize();
	//		_velocity *= speed;
	//	}

	//	protected override void Initialize()
	//	{
	//		Entity.Rotation = _velocity.ToAngle();
	//	}

	//	protected override void Update()
	//	{
	//		Entity.Position += (_velocity * (float)GameTime.ElapsedGameTime.TotalMilliseconds);
	//	}
	//}

	//public abstract class MissileSpawningStrategy
	//{
	//	public abstract IEnumerator Begin(MissileSpawner projectileSpawner);
	//}

	//public class SequencedCircularMissileSpawnerStrategy : MissileSpawningStrategy
	//{
	//	private int _count;

	//	private int _delay;

	//	private int _sequenceFrequencyInMS;

	//	private int _frequencyInMS;

	//	public SequencedCircularMissileSpawnerStrategy(
	//		int count,
	//		int initialDelayInMS,
	//		int sequenceFrequencyInMS,
	//		int frequencyInMS)
	//	{
	//		_count = count;
	//		_delay = initialDelayInMS;
	//		_sequenceFrequencyInMS = sequenceFrequencyInMS;
	//		_frequencyInMS = frequencyInMS;
	//	}

	//	public override IEnumerator Begin(MissileSpawner projectileSpawner)
	//	{
	//		yield return Wait.Duration(_delay);
	//		while (true)
	//		{
	//			var degreesPerSpawn = 360 / _count;
	//			for (int r = 0; r < _count; r++)
	//			{
	//				var a = degreesPerSpawn * r;
	//				projectileSpawner.Spawn(Vector2Helper.FromAngle(MathHelper.ToRadians(a)));
	//				yield return Wait.Duration(_sequenceFrequencyInMS);
	//			}
	//			yield return Wait.Duration(_frequencyInMS);
	//		}
	//	}
	//}
}
