using Coldsteel;
using Coldsteel.Animations;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace LD46.Gameplay
{
	enum TurretyType
	{
		BlueTurret,
		Green,
		Red,
		Dark
	}

	class Turret : Entity
	{
		public TurretyType turretType { get; }
		public bool Demo { get; }

		public Turret(TurretyType type, bool demo = false)
		{
			turretType = type;
			Demo = demo;

			var sprite = new Sprite(
				turretType == TurretyType.BlueTurret ? Assets.Texture2D.tower :
				turretType == TurretyType.Green ? Assets.Texture2D.greentower :
				turretType == TurretyType.Red ? Assets.Texture2D.redtower :
				turretType == TurretyType.Dark ? Assets.Texture2D.darktower :
				Assets.Texture2D.tower,
				frameSize: new Size(12, 12)
			)
			{
				FrameIndex = 0,
				Origin = new Vector2(6, 6),
				RenderingLayerName = demo ? "hud" : null,
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

			if (!demo)
			{
				new MissileSpawner(
					turretType == TurretyType.BlueTurret ? Settings.BlueROF :
					turretType == TurretyType.Green ? Settings.GreenROF :
					turretType == TurretyType.Red ? Settings.RedROF :
					turretType == TurretyType.Dark ? Settings.BlackROF :
					0
					,
					turretType == TurretyType.BlueTurret ? Color.White :
					turretType == TurretyType.Green ? Color.LightGreen :
					turretType == TurretyType.Red ? Color.Red :
					turretType == TurretyType.Dark ? Color.Gray :
					Color.White, turretType).AddToEntity(this);
			}
		}

		public SpriteAnimator Animator { get; }

		public class MissileSpawner : Behavior
		{
			private int freq;
			private double timeSinceLastFire;
			private Color color;
			private TurretyType turretType;
			private int range = 0;

			public static float BasicSpeed => 0.2f;

			public MissileSpawner(int freq, Color color, TurretyType type)
			{
				this.turretType = type;
				this.freq = turretType == TurretyType.BlueTurret ? Settings.BlueROF :
						turretType == TurretyType.Green ? Settings.GreenROF :
						turretType == TurretyType.Red ? Settings.RedROF :
						turretType == TurretyType.Dark ? Settings.BlackROF :
						0;

				this.range = turretType == TurretyType.BlueTurret ? Settings.BlueRange :
						turretType == TurretyType.Green ? Settings.GreenRange :
						turretType == TurretyType.Red ? Settings.RedRange :
						turretType == TurretyType.Dark ? Settings.BlackRange :
						0;

				this.color = color;
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
					.Where(c => c.D < range)
					.OrderBy(c => c.D)
					.Select(c => c.E)
					.FirstOrDefault();

				if (target == null) return;

				var missile = new Missile(BasicSpeed, color);
				missile.Position = this.Entity.Position;
				missile.AddComponent(new SeekingMissileBehavior(missile, target, 8,
						turretType == TurretyType.BlueTurret ? Settings.BlueDamage :
						turretType == TurretyType.Green ? Settings.GreenDamage :
						turretType == TurretyType.Red ? Settings.RedDamage :
						turretType == TurretyType.Dark ? Settings.BlackDamage :
						0
					));
				
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
			public int dmg;

			public SeekingMissileBehavior(Missile missile, Slime target, int hitRadius, int dmg)
			{
				this.missile = missile;
				this.target = target;
				this.hitRadius = hitRadius;
				this.dmg = dmg;
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
					target.Hit(dmg);
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

	class TurretPicker : Entity
	{
		private Turret turret;

		public TurretPicker(TurretyType type)
		{
			turret = new Turret(type, true);
			turret.Scale = 2.0f;
			this.Position =
				type == TurretyType.BlueTurret ? new Vector2(-64, 160) :
				type == TurretyType.Green ? new Vector2(-0, 160) :
				type == TurretyType.Red ? new Vector2(64, 160) :
				type == TurretyType.Dark ? new Vector2(128, 160) :
				Vector2.Zero;
			this.AddChild(turret);
			Sprite = new Sprite(Assets.Texture2D.selector, renderingLayerName: "hud")
			{
				Origin = new Vector2(24, 24)
			};
			Sprite.Enabled = false;
			this.AddComponent(Sprite);
		}

		public bool Selected
		{
			get => Sprite.Enabled;
			set => Sprite.Enabled = value;
		}


		public Sprite Sprite { get; }

		internal void SetColor(Color color)
		{
			Sprite.Color = color;
			turret.GetComponent<Sprite>().Color = color;
		}
	}
}
