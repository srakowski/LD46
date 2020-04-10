// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework.Audio;
using System.Linq;

namespace Coldsteel
{
	public class AudioSource : Component
	{
		private string _soundEffectAssetName;
		private Asset<SoundEffect> _soundEffect;

		public AudioSource() { }

		public AudioSource(string soundEffectAssetName)
		{
			_soundEffectAssetName = soundEffectAssetName;
			_soundEffect = null;
		}

		public void Play() => _soundEffect.GetValue().Play();

		public void Play(float volume, float pitch, float pan) => _soundEffect.GetValue().Play(volume, pitch, pan);

		private protected override void Activated()
		{
			_soundEffect = Scene.Assets.FirstOrDefault(a => a.Name == _soundEffectAssetName) as Asset<SoundEffect>;
		}

		private protected override void Deactivated()
		{
			_soundEffect = null;
		}
	}
}
