// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Coldsteel.UI.Elements
{
	public class Div : Element, IElementCollection
	{
		private readonly ICollection<Element> _elements = new List<Element>();

		public Color BackgroundColor { get; set; } = Color.Transparent;

		public Color BorderColor { get; set; } = Color.Transparent;

		public int BorderWidth { get; set; } = 0;

		public BorderRadius BorderRadius { get; set; } = default;

		public Div Add(Element element)
		{
			_elements.Add(element);
			return this;
		}

		public Div Add(params Element[] elements)
		{
			foreach (var element in elements)
				_elements.Add(element);
			return this;
		}

		public IEnumerator<Element> GetEnumerator() => _elements.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => _elements.GetEnumerator();

		public static Div New => new Div();

		public Div Configure(Action<Div> configure)
		{
			configure(this);
			return this;
		}
	}

	public struct BorderRadius
	{
		public BorderRadius(int radius)
			: this(radius, radius, radius, radius)
		{
		}

		public BorderRadius(int tl, int tr, int bl, int br)
		{
			TopLeft = tl;
			TopRight = tr;
			BottomLeft = bl;
			BottomRight = br;
		}

		public int TopLeft;
		public int TopRight;
		public int BottomLeft;
		public int BottomRight;

		public static implicit operator BorderRadius(int value) => new BorderRadius(value);
	}
}

//internal class Box : RenderNode
//{
//	public Box(
//		MGRectangle bounds,
//		RenderNode[] children,
//		MGColor? bgColor = null,
//		MGColor? borderColor = null,
//		int borderWidth = 0,
//		BorderRadius borderRadius = default
//		)
//	{
//		Bounds = bounds;
//		Children = children;
//		BackgroundColor = bgColor ?? MGColor.Transparent;
//		BorderColor = borderColor ?? MGColor.Transparent;
//		BorderWidth = borderWidth;
//		BorderRadius = borderRadius;
//	}

//	public MGRectangle Bounds { get; }

//	public RenderNode[] Children { get; }

//	public MGColor BackgroundColor { get; }

//	public MGColor BorderColor { get; }

//	public int BorderWidth { get; }

//	public BorderRadius BorderRadius { get; }
//}