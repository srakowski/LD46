using Coldsteel;
using Coldsteel.Animations;
using Microsoft.Xna.Framework;

namespace LD46.Gameplay
{
	class Player : Entity
	{
		public Player()
		{
			this.AddCamera();

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

			new PlayerController(this).AddToEntity(this);
		}

		public SpriteAnimator Animator { get; }
	}

	internal class PlayerController : Behavior
	{
		private Player player;
		private Controls controls;
		private char d = 'F';

		public PlayerController(Player player)
		{
			this.player = player;
		}

		protected override void Initialize()
		{
			controls = Controls.Map(this);
		}

		protected override void Update()
		{
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

			if (idle)
			{
				player.Animator.Animate($"idle{d}");
			}
		}
	}
}
