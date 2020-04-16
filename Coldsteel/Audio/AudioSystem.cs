// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Linq;
using MGAudioEmitter = Microsoft.Xna.Framework.Audio.AudioEmitter;
using MGAudioListener = Microsoft.Xna.Framework.Audio.AudioListener;

namespace Coldsteel.Audio
{
	internal class AudioSystem : SystemBase<AudioComponent>
	{
		private bool _killSoundEffects = false;

		private readonly Dictionary<Scene, List<IEnumerator<bool>>> _applyContinuationsByScene
			= new Dictionary<Scene, List<IEnumerator<bool>>>();

		public float Scalar = 500f;

		public AudioSystem(Game game, Engine engine) : base(game, engine)
		{
		}

		public override void Initialize()
		{
			base.Initialize();
			Engine.SceneManager.OnSceneChanging += SceneManager_OnSceneChanging;
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

		internal void PlaySoundEffect(MGAudioEmitter emitter, string soundEffectName)
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

		internal void PlaySong(string songName)
		{
			var activeScene = Engine.SceneManager.ActiveScene;
			if (activeScene == null) return;
			if (ActiveComponents == null) return;

			var song = activeScene.Assets?.FirstOrDefault(a => a.Name == songName) as Asset<Song>;
			if (song == null || !song.IsLoaded) return;

			MediaPlayer.Play(song.GetValue());
		}

		internal void StopSong()
		{
			MediaPlayer.Stop();
		}

		private IEnumerator<bool> ContinueApply(SoundEffectInstance sei, MGAudioListener[] listeners, MGAudioEmitter emitter)
		{
			while (sei.State != SoundState.Stopped && !_killSoundEffects)
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
			if (scene == null) return null;
			return _applyContinuationsByScene.ContainsKey(scene)
				? _applyContinuationsByScene[scene]
				: (_applyContinuationsByScene[scene] = new List<IEnumerator<bool>>());
		}

		private void SceneManager_OnSceneChanging(object sender, SceneChangingEventArgs e)
		{
			MediaPlayer.Stop();
			var soundEffectContinuations = GetApplyContinuationsByScene(e.FromScene);
			if (soundEffectContinuations is null) return;
			_killSoundEffects = true;
			foreach (var con in soundEffectContinuations)
				con.MoveNext();
			_killSoundEffects = false;
			soundEffectContinuations.Clear();
		}
	}
}
