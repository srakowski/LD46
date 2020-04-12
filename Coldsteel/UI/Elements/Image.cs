// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Coldsteel.UI.Elements
{
	public class Image : Element
	{
		public string Source { get; set; }

		public static Image New => new Image();

		public Image Configure(Action<Image> configure)
		{
			configure(this);
			return this;
		}
	}
}
