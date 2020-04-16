// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Coldsteel.Audio
{
	public class MusicPlayer : AudioComponent
	{
		public void Play(string songName)
		{
			Engine.AudioSystem.PlaySong(songName);
		}

		public void Stop()
		{
			Engine.AudioSystem.StopSong();
		}
	}
}
