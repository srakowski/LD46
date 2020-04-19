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
		}

		public SpriteAnimator Animator { get; }
	}
}
