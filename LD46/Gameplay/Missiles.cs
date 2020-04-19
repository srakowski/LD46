using Coldsteel;
using Coldsteel.Animations;
using Microsoft.Xna.Framework;
using System.Linq;

namespace LD46.Gameplay
{
	class Missile : Entity
	{
		public Missile(float speed, Color color)
		{
			Speed = speed;

			var sprite = new Sprite(
				Assets.Texture2D.magicMissile,
				frameSize: new Size(8, 16)
			)
			{
				RenderingLayerName = "missiles",
				FrameIndex = 0,
				Origin = new Vector2(4, 8),
				Color = color
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

		public SpriteAnimator Animator { get; }

		public float Speed { get; }
		public Vector2 Velocity { get; internal set; }
	}
}
