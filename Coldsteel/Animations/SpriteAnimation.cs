// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Coldsteel.Animations
{
	public class SpriteAnimation
	{
		public SpriteAnimation(string name, params Frame[] frames)
		{
			Name = name;
			Frames = frames;
		}

		public string Name { get; }

		public Frame[] Frames { get; }
	}
}
