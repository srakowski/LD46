using Coldsteel;
using Coldsteel.Animations;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace LD46.GameplayOld
{
	class Player : Entity
	{
		public Player(Camera camera, Map map)
		{
			var sprite = new Sprite(
				Assets.Texture2D.hero,
				frameSize: new Size(16, 16)
			)
			{
				FrameIndex = 0,
				Origin = new Vector2(8, 8),
			}
			.AddToEntity(this);

			Animator = new SpriteAnimator(sprite)
				.AddSpriteAnimation("idleF", (0, 250), (1, 250))
				.AddSpriteAnimation("walkF", (2, 200), (3, 200))
				.AddSpriteAnimation("idleR", (4, 250), (5, 250))
				.AddSpriteAnimation("walkR", (7, 150), (5, 100), (6, 150))
				.AddSpriteAnimation("idleL", (8, 250), (9, 250))
				.AddSpriteAnimation("walkL", (11, 150), (9, 100), (10, 150))
				.AddSpriteAnimation("idleB", (12, 250), (13, 250))
				.AddSpriteAnimation("walkB", (14, 200), (15, 200))
				.AddToEntity(this);

			Animator.Animate("idleF");

			MissileSpawner = new MissileSpawner(300).AddToEntity(this);

			new PlayerController(this, camera, map).AddToEntity(this);
		}

		public SpriteAnimator Animator { get; }

		public MissileSpawner MissileSpawner { get; }
	}

	internal class PlayerController : Behavior
	{
		private Player player;
		private Camera camera;
		private Controls controls;
		private char d = 'F';
		private Map map;

		public PlayerController(Player player, Camera camera, Map map)
		{
			this.player = player;
			this.camera = camera;
			this.map = map;
		}

		protected override void Initialize()
		{
			controls = Controls.Map(this);
		}

		protected override void Update()
		{
			//if (controls.PlayerMove.WasPushed())
			//{
			//	var target = controls.PlayerMoveLocation.GetPosition();
			//	player.Position = camera.ToWorldCoords(ScreenToView(target));
			//}

			//var speed = 0.09f;
			//if (controls.Up.IsDown())
			//{
			//	camera.Entity.Position += (new Vector2(0, -1) * speed * (float)GameTime.ElapsedGameTime.TotalMilliseconds);
			//}
			//if (controls.Down.IsDown())
			//{
			//	camera.Entity.Position += (new Vector2(0, 1) * speed * (float)GameTime.ElapsedGameTime.TotalMilliseconds);
			//}
			//if (controls.Left.IsDown())
			//{
			//	camera.Entity.Position += (new Vector2(-1, 0) * speed * (float)GameTime.ElapsedGameTime.TotalMilliseconds);
			//}
			//if (controls.Right.IsDown())
			//{
			//	camera.Entity.Position += (new Vector2(1, 0) * speed * (float)GameTime.ElapsedGameTime.TotalMilliseconds);
			//}

			var speed = 0.09f;
			var idle = true;

			if (controls.Up.IsDown())
			{
				player.Position += (new Vector2(0, -1) * speed * (float)GameTime.ElapsedGameTime.TotalMilliseconds);
				if (d == 'B') player.Animator.Animate("walkB");
				d = 'B';
				idle = false;
			}
			else if (controls.Down.IsDown())
			{
				player.Position += (new Vector2(0, 1) * speed * (float)GameTime.ElapsedGameTime.TotalMilliseconds);
				if (d == 'F') player.Animator.Animate("walkF");
				d = 'F';
				idle = false;
			}

			if (controls.Left.IsDown())
			{
				player.Position += (new Vector2(-1, 0) * speed * (float)GameTime.ElapsedGameTime.TotalMilliseconds);
				if (d == 'L') player.Animator.Animate("walkL");
				d = 'L';
				idle = false;
			}
			else if (controls.Right.IsDown())
			{
				player.Position += (new Vector2(1, 0) * speed * (float)GameTime.ElapsedGameTime.TotalMilliseconds);
				if (d == 'R') player.Animator.Animate("walkR");
				d = 'R';
				idle = false;
			}


			if (controls.Fire.IsDown())
			{
				if (spawnMissile == null) spawnMissile = SpawnMissile;
				player.MissileSpawner.Spawn(spawnMissile);
			}

			//if (controls.CenterPlayer.IsDown())
			//{
				camera.Entity.Position = player.Position.ToPoint().ToVector2();
			//}

			if (idle)
			{
				player.Animator.Animate($"idle{d}");
			}
		}

		private Func<Missile> spawnMissile;

		private Missile SpawnMissile()
		{
			var target = Scene.Entities.OfType<Slime>()
				.OrderBy(m => Vector2.Distance(m.Position, Entity.Position))
				.FirstOrDefault();
			if (target == null) return null;
			return new Missile(player.Position, target, Missile.BasicSpeed, 8);
		}
	}
}
