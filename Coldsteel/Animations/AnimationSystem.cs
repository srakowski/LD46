// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework;

namespace Coldsteel.Animations
{
	internal class AnimationSystem : SystemBase<SpriteAnimator>
	{
		public AnimationSystem(Game game, Engine engine) : base(game, engine)
		{
		}

		public override void Update(GameTime gameTime)
		{
			var spriteAnimators = ActiveComponents;
			if (spriteAnimators == null) return;
			foreach (var sa in spriteAnimators)
				sa.Update(gameTime);
		}
	}
}
