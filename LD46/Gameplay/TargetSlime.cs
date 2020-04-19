using Coldsteel;
using Coldsteel.Animations;
using Microsoft.Xna.Framework;

namespace LD46.Gameplay
{
	class TargetSlime : Entity
	{
		public TargetSlime()
		{
			var sprite = new Sprite(
				Assets.Texture2D.targetslime,
				frameSize: new Size(16, 16)
			)
			{
				FrameIndex = 0,
				Origin = new Vector2(8, 8),
			}
			.AddToEntity(this);

			var animator = new SpriteAnimator(sprite)
				.AddSpriteAnimation("idle", (0, 250), (1, 250))
				.AddToEntity(this);

			animator.Animate("idle");
		}
	}
}
