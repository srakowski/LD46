// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework;

namespace Coldsteel
{
	public struct Size
	{
		public Size(int width, int height)
		{
			Width = width;
			Height = height;
		}

		public readonly int Width;
		public readonly int Height;

		public static implicit operator Point(Size size) => new Point(size.Width, size.Height);
	}
}
