// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework;
using System;

namespace Coldsteel.UI.Elements
{
	public enum Align
	{
		Near,
		Center,
		Far,
	}

	public class Text : Element
	{
		public string Value { get; set; } = "";

		public string Font { get; set; } = "Comic Sans MS";

		public int Size { get; set; } = 14;

		public Color Color { get; set; } = Color.White;

		public Align Align { get; set; } = Align.Near;

		public Align VerticalAlign { get; set; } = Align.Near;

		public bool Bold { get; set; } = false;

		public bool Italic { get; set; } = false;

		public static Text New => new Text();

		public Text Configure(Action<Text> configure)
		{
			configure(this);
			return this;
		}
	}
}
