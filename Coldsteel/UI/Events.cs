// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework;
using System;

namespace Coldsteel.UI
{
	public class MouseMovementEventArgs : EventArgs
	{
		public MouseMovementEventArgs(Point position)
		{
			Position = position;
		}

		public Point Position { get; }
	}

	public class MouseClickEventArgs : EventArgs
	{
		public MouseClickEventArgs(Point position)
		{
			Position = position;
		}

		public Point Position { get; }

		public bool Handled { get; set; } = false;
	}
}
