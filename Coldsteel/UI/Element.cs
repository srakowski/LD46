// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Coldsteel.UI
{
	public abstract class Element
	{
		public Anchor Anchor { get; set; } = Anchor.TopLeft;

		public Dock Dock { get; set; } = Dock.None;

		public Anchor Origin { get; set; } = Anchor.TopLeft;

		public Point Offset { get; set; } = Point.Zero;

		public int Width { get; set; } = 100;

		public int Height { get; set; } = 100;

		public Rectangle Bounds { get; private set; }

		internal void UpdateBounds(Rectangle containerBounds, AnchorPoints anchorPoints)
		{
			var location = anchorPoints.GetAnchorPoint(Anchor);
			var size = new Point(Width, Height);
			switch (Dock)
			{
				case Dock.Fill:
					location = containerBounds.Location;
					size = containerBounds.Size;
					break;

				case Dock.Top:
					location = containerBounds.Location;
					size.X = containerBounds.Width;
					break;

				case Dock.Left:
					location = containerBounds.Location;
					size.Y = containerBounds.Height;
					break;

				case Dock.Right:
					location.Y = containerBounds.Y;
					location.X = containerBounds.Right - size.X;
					size.Y = containerBounds.Height;
					break;

				case Dock.Bottom:
					location.X = containerBounds.X;
					location.Y = containerBounds.Bottom - size.Y;
					size.X = containerBounds.Width;
					break;
			}

			var originPoints = new AnchorPoints(new Rectangle(Point.Zero, size));
			var origin = originPoints.GetAnchorPoint(Origin);

			location = (location - origin) + Offset;
			Bounds = new Rectangle(location, size);
		}
	}

	public interface IElementCollection
	{
		IEnumerable<Element> Elements { get; }
	}

	public enum Anchor
	{
		TopLeft,
		TopCenter,
		TopRight,
		CenterLeft,
		Center,
		CenterRight,
		BottomLeft,
		BottomCenter,
		BottomRight
	}

	internal struct AnchorPoints
	{
		public AnchorPoints(Rectangle bounds)
		{
			TopLeft = new Point(bounds.Left, bounds.Top);
			TopCenter = new Point(bounds.Center.X, bounds.Top);
			TopRight = new Point(bounds.Right, bounds.Top);
			CenterLeft = new Point(bounds.Left, bounds.Center.Y);
			Center = bounds.Center;
			CenterRight = new Point(bounds.Right, bounds.Center.Y);
			BottomLeft = new Point(bounds.Left, bounds.Bottom);
			BottomCenter = new Point(bounds.Center.X, bounds.Bottom);
			BottomRight = new Point(bounds.Right, bounds.Bottom);
		}

		public Point TopLeft;
		public Point TopCenter;
		public Point TopRight;
		public Point CenterLeft;
		public Point Center;
		public Point CenterRight;
		public Point BottomLeft;
		public Point BottomCenter;
		public Point BottomRight;

		public Point GetAnchorPoint(Anchor anchor)
		{
			switch (anchor)
			{
				case Anchor.TopLeft: return TopLeft;
				case Anchor.TopCenter: return TopCenter;
				case Anchor.TopRight: return TopRight;
				case Anchor.CenterLeft: return CenterLeft;
				case Anchor.Center: return Center;
				case Anchor.CenterRight: return CenterRight;
				case Anchor.BottomLeft: return BottomLeft;
				case Anchor.BottomCenter: return BottomCenter;
				case Anchor.BottomRight: return BottomRight;
				default: return Center;
			}
		}
	}

	public enum Dock
	{
		None,
		Top,
		Left,
		Fill,
		Right,
		Bottom
	}
}
