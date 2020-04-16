// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework;
using MGAudioEmitter = Microsoft.Xna.Framework.Audio.AudioEmitter;

namespace Coldsteel.Audio
{
	public class AudioEmitter : AudioComponent
	{
		public AudioEmitter()
		{
			Emitter = new MGAudioEmitter();
		}

		internal readonly MGAudioEmitter Emitter;

		internal override void Update()
		{
			Emitter.Position = new Vector3(Entity.Position / Engine.AudioSystem.Scalar, -3f);
		}

		public AudioEmitter Play(string soundEffectName)
		{
			Engine.AudioSystem.PlaySoundEffect(this, soundEffectName);
			return this;
		}
	}
}
