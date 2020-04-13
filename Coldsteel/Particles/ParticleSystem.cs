// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Coldsteel.Particles
{
	internal class ParticleSystem : SystemBase<ParticleEmitter>
	{
		private readonly Particle[] _particles = new Particle[4000];

		private int _particleIndex = 0;

		public ParticleSystem(Game game, Engine engine) : base(game, engine)
		{
		}

		public override void Update(GameTime gameTime)
		{
			for (int i = 0; i < _particles.Length; i++)
			{
				if (_particles[i].Dead) continue;
				_particles[i] = _particles[i].Update(gameTime);
			}
		}

		internal IEnumerable<Particle> Particles => _particles;

		internal void AddParticles(IEnumerable<Particle> particles)
		{
			foreach (var particle in particles)
			{
				_particles[_particleIndex % _particles.Length] = particle;
				_particleIndex++;
			}
		}
	}
}
