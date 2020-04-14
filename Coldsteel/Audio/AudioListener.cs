// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework;
using MGAudioListener = Microsoft.Xna.Framework.Audio.AudioListener;

namespace Coldsteel.Audio
{
	public class AudioListener : AudioComponent
	{
		public AudioListener()
		{
			Listener = new MGAudioListener();
		}

		internal readonly MGAudioListener Listener;

		internal override void Update()
		{
			Listener.Position = new Vector3(Entity.Position / Engine.AudioSystem.Scalar, 1);
		}
	}
}
