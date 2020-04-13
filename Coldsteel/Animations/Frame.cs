// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Coldsteel.Animations
{
	public struct Frame
	{
		public Frame(int index, int duration)
		{
			Index = index;
			Duration = duration;
		}

		public readonly int Index;
		public readonly int Duration;
	}
}
