// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework;

namespace Coldsteel
{
	internal class BehaviorSystem : SystemBase<Behavior>
    {
        public BehaviorSystem(Game game, Engine engine) : base(game, engine)
        {
        }

        public override void Update(GameTime gameTime)
        {
			var behaviors = ActiveComponents;
			if (behaviors == null) return;

            foreach (var behavior in behaviors.ToArray())
            {
                behavior.Update(gameTime);
            }

            foreach (var behavior in behaviors.ToArray())
            {
                behavior.UpdateCoroutines(gameTime);
            }
        }
    }
}
