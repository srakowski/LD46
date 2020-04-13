// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coldsteel.Particles
{
	public class ParticleEmitter : Component
	{
		private Asset<Texture2D> _texture;
		
		public ParticleEmitter(string assetName, string spriteLayerName)
		{
			AssetName = assetName;
			SpriteLayerName = spriteLayerName;
			ColorGen = new FixedValue<Color>(Color.White);
			VelocityGen = new RandomVector2(-0.3f, 0.3f, -0.3f, 0.3f);
			TtlGen = new RandomDouble(1000, 4000);
		}

		public string AssetName;
		// Position
		public ParticlePropertyFactory<Color> ColorGen;
		public ParticlePropertyFactory<float> RotationGen;
		public Vector2 Origin = Vector2.Zero;
		public ParticlePropertyFactory<float> ScaleGen;
		public ParticlePropertyFactory<double> TtlGen;
		public ParticlePropertyFactory<Vector2> VelocityGen;
		public ParticlePropertyFactory<float> ScaleVelocityGen;
		public ParticlePropertyFactory<float> RotationVelocityGen;
		public string SpriteLayerName;
		public float LayerDepth = 1f;
	
		private protected override void Activated()
		{
			Engine.ParticleSystem.AddComponent(Scene, this);
			_texture = Scene.Assets.FirstOrDefault(a => a.Name == AssetName) as Asset<Texture2D>;
		}

		private protected override void Deactivated()
		{
			_texture = null;
			Engine.ParticleSystem.RemoveComponent(Scene, this);
		}

		public void Emit(int count)
		{
			Engine.ParticleSystem.AddParticles(ParticleGenerator(count, new Random()));
		}

		private IEnumerable<Particle> ParticleGenerator(int count, Random random)
        {
			for (var i = 0; i < count; i++)
				yield return new Particle()
				{
					Texture = _texture,
                    Position = Entity.GlobalPosition,
					Color = ColorGen?.Create(random) ?? Color.White,
					Rotation = RotationGen?.Create(random) ?? 0f,
					Origin = Origin,
					Scale = ScaleGen?.Create(random) ?? 1f,
					Ttl = TtlGen?.Create(random) ?? 1000,
					Velocity = VelocityGen?.Create(random) ?? new Vector2(0.3f, 0.3f),
					ScaleVelocity = ScaleVelocityGen?.Create(random) ?? 0f,
					RotationVelocity = RotationVelocityGen?.Create(random) ?? 0f,
					SpriteLayerName = SpriteLayerName,
					LayerDepth = LayerDepth
				};
        }
	}

	public abstract class ParticlePropertyFactory<T>
	{
		public abstract T Create(Random random);
	}

	public class RandomInt : ParticlePropertyFactory<int>
	{
		public RandomInt(int min, int max)
		{
			MinValue = min;
			MaxValue = max;
		}
		public int MinValue { get; set; } = 0;
		public int MaxValue { get; set; } = 100;
		public override int Create(Random random) =>
			random.Next(MinValue, MaxValue + 1);
	}

	public class RandomDouble : ParticlePropertyFactory<double>
	{
		public RandomDouble(double min, double max)
		{
			MinValue = min;
			MaxValue = max;
		}
		public double MinValue { get; set; } = 0;
		public double MaxValue { get; set; } = 1;
		public override double Create(Random random)
		{
			var scaler = MaxValue - MinValue;
			return (random.NextDouble() * scaler) - MinValue;
		}
	}

	public class RandomFloat : ParticlePropertyFactory<float>
	{
		public RandomFloat(float min, float max)
		{
			MinValue = min;
			MaxValue = max;
		}
		public float MinValue { get; set; } = 0;
		public float MaxValue { get; set; } = 1;
		public override float Create(Random random)
		{
			var scaler = MaxValue - MinValue;
			return ((float)random.NextDouble() * scaler) - MinValue;
		}
	}

	public class FixedValue<T> : ParticlePropertyFactory<T>
	{
		public FixedValue(T value)
		{
			Value = value;
		}
		public T Value { get; set; } = default;
		public override T Create(Random random) => Value;
	}

	public class RandomVector2 : ParticlePropertyFactory<Vector2>
	{
		public RandomVector2(float minX, float maxX, float minY, float maxY)
		{
			MinX = minX;
			MaxX = maxX;
			MinY = minY;
			MaxY = maxY;
		}

		public float MinX { get; set; } = -0.1f;
		public float MaxX { get; set; } = 0.1f;
		public float MinY { get; set; } = -0.1f;
		public float MaxY { get; set; } = 0.1f;

		public override Vector2 Create(Random random)
		{
			var minX = (int)(MinX * 1000);
			var maxX = (int)(MaxX * 1000);
			var minY = (int)(MinY * 1000);
			var maxY = (int)(MaxY * 1000);
			var x = random.Next(minX, maxX + 1) / 1000f;
			var y = random.Next(minY, maxY + 1) / 1000f;
			return new Vector2(x, y);
		}
	}
}
