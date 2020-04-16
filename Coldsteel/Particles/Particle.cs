// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Coldsteel.Particles
{
	internal struct Particle : IRenderer
	{
		public Asset<Texture2D> Texture;
		public Vector2 Position;
		public Color Color;
		public float Rotation;
		public Vector2 Origin;
		public float Scale;
		public double Ttl;
		public Vector2 Velocity;
		public float ScaleVelocity;
		public float RotationVelocity;
		public string RenderingLayerName;
		public float LayerDepth;

		string IRenderer.RenderingLayerName => RenderingLayerName;

		public bool Dead => Ttl <= 0;
		public bool Enabled => !Dead;

		public Particle Update(GameTime gameTime)
		{
			var delta = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
			Ttl -= delta;
			if (Ttl > 0)
			{
				Position += Velocity * delta;
				Scale += ScaleVelocity * delta;
				Rotation += RotationVelocity * delta;
			}
			return this;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (!(Texture?.IsLoaded ?? false) || !Enabled) return;
			spriteBatch.Draw(
				Texture.GetValue(),
				Position,
				null,
				Color,
				Rotation,
				Origin,
				Scale,
				SpriteEffects.None,
				LayerDepth
			);
		}
	}
}
