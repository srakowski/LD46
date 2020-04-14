// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using MGAudioEmitter = Microsoft.Xna.Framework.Audio.AudioEmitter;
using MGAudioListener = Microsoft.Xna.Framework.Audio.AudioListener;

namespace Coldsteel.Audio
{
	internal class AudioSystem : SystemBase<AudioComponent>
	{
		private readonly Dictionary<Scene, List<IEnumerator<bool>>> _applyContinuationsByScene
			= new Dictionary<Scene, List<IEnumerator<bool>>>();

		public float Scalar = 200f;

		public AudioSystem(Game game, Engine engine) : base(game, engine)
		{
		}

		public override void Update(GameTime gameTime)
		{
			if (Engine.SceneManager.ActiveScene == null) return;
			if (ActiveComponents == null) return;

			foreach (var component in ActiveComponents)
				component.Update();

			var continuations = GetApplyContinuationsByScene(Engine.SceneManager.ActiveScene);
			foreach (var cont in continuations) cont.MoveNext();
			continuations.RemoveAll(r => !r.Current);
		}

		internal void Play(MGAudioEmitter emitter, string soundEffectName)
		{
			var activeScene = Engine.SceneManager.ActiveScene;
			if (activeScene == null) return;
			if (ActiveComponents == null) return;

			var se = activeScene.Assets?.FirstOrDefault(a => a.Name == soundEffectName) as Asset<SoundEffect>;
			if (se == null || !se.IsLoaded) return;

			var sei = se.GetValue().CreateInstance();

			var listeners = ActiveComponents.OfType<AudioListener>().Select(l => l.Listener).ToArray();

			sei.Apply3D(
				listeners,
				emitter
			);

			sei.Play();

			var continuations = GetApplyContinuationsByScene(activeScene);
			continuations.Add(ContinueApply(sei, listeners, emitter));
		}

		private IEnumerator<bool> ContinueApply(SoundEffectInstance sei, MGAudioListener[] listeners, MGAudioEmitter emitter)
		{
			while (sei.State != SoundState.Stopped)
			{
				sei.Apply3D(
					listeners,
					emitter
				);
				yield return true;
			}
			sei.Dispose();
			yield return false;
		}

		private List<IEnumerator<bool>> GetApplyContinuationsByScene(Scene scene)
		{
			return _applyContinuationsByScene.ContainsKey(scene)
				? _applyContinuationsByScene[scene]
				: (_applyContinuationsByScene[scene] = new List<IEnumerator<bool>>());
		}
	}
}
