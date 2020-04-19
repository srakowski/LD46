﻿using Coldsteel;
using Coldsteel.Animations;
using Microsoft.Xna.Framework;

namespace LD46.Gameplay
{
	class Monster : Entity
	{
		public Monster()
		{
			this.AddCamera();

			var sprite = new Sprite(
				Assets.Texture2D.slime,
				frameSize: new Size(16, 16)
			)
			{
				FrameIndex = 0,
				Origin = new Vector2(8, 8),
			}
			.AddToEntity(this);

			Animator = new SpriteAnimator(sprite)
				.AddSpriteAnimation("idle", (0, 1000), (1, 500))
				.AddToEntity(this);

			Animator.Animate("idle");
		}

		public SpriteAnimator Animator { get; }
	}
}