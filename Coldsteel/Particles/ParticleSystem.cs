// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Coldsteel.Particles
{
	internal class ParticleSystem : GameComponent
	{
		private readonly Dictionary<Scene, List<ParticleEmitter>> _emittersByScene = new Dictionary<Scene, List<ParticleEmitter>>();

		private readonly Particle[] _particles = new Particle[4000];

		private int _particleIndex = 0;

		private readonly Engine _engine;

		public ParticleSystem(Game game, Engine engine) : base(game)
		{
			_engine = engine;
			game.Components.Add(this);
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

		internal void AddEmitter(Scene scene, ParticleEmitter emitter)
		{
			var emitterList = GetEmitterListForScene(scene);
			emitterList.Add(emitter);
		}

		internal void RemoveEmitter(Scene scene, ParticleEmitter emitter)
		{
			var emitterList = GetEmitterListForScene(scene);
			emitterList.Remove(emitter);
		}

		private List<ParticleEmitter> GetEmitterListForScene(Scene scene)
		{
			return _emittersByScene.ContainsKey(scene)
				? _emittersByScene[scene]
				: (_emittersByScene[scene] = new List<ParticleEmitter>());
		}

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
