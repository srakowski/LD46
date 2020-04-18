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
				.AddSpriteAnimation("idleF", (0, 1000), (1, 1500))
				.AddSpriteAnimation("walkF", (2, 200), (3, 200))
				.AddSpriteAnimation("idleR", (4, 1000), (5, 1500))
				.AddSpriteAnimation("walkR", (7, 150), (5, 100), (6, 150))
				.AddSpriteAnimation("idleL", (8, 1000), (9, 1500))
				.AddSpriteAnimation("walkL", (11, 150), (9, 100), (10, 150))
				.AddSpriteAnimation("idleB", (12, 1000), (13, 1500))
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
			if (controls.Up.IsDown())
			{
				if (d == 'B') player.Animator.Animate("walkB");
				d = 'B';
			}
			else if (controls.Down.IsDown())
			{
				if (d == 'F') player.Animator.Animate("walkF");
				d = 'F';
			}
			else if (controls.Left.IsDown())
			{
				if (d == 'L') player.Animator.Animate("walkL");
				d = 'L';
			}
			else if (controls.Right.IsDown())
			{
				if (d == 'R') player.Animator.Animate("walkR");
				d = 'R';
			}
			else
			{
				player.Animator.Animate($"idle{d}");
			}
		}
	}
}
