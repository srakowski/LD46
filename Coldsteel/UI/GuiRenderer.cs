// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using MGRectangle = Microsoft.Xna.Framework.Rectangle;
using MGColor = Microsoft.Xna.Framework.Color;
using System.Drawing.Text;

namespace Coldsteel.UI
{
	internal class RenderTree
	{
		public RenderTree(Box root)
		{
			Root = root;
		}

		public Box Root { get; }
	}

	internal abstract class RenderNode
	{
		public void Match(
			Action<Text> text,
			Action<Box> box)
		{
			switch (this)
			{
				case Text t: text(t); break;
				case Box b: box(b); break;
			}
		}
	}

	internal class Text : RenderNode
	{
		public MGRectangle Bounds { get; }

		public MGColor Color { get; }

		public string Value { get; }
	}

	internal struct BorderRadius
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
	}

	internal class Box : RenderNode
	{
		public Box(
			MGRectangle bounds,
			RenderNode[] children,
			MGColor? bgColor = null,
			MGColor? borderColor = null,
			int borderWidth = 0,
			BorderRadius borderRadius = default
			)
		{
			Bounds = bounds;
			Children = children;
			BackgroundColor = bgColor ?? MGColor.Transparent;
			BorderColor = borderColor ?? MGColor.Transparent;
			BorderWidth = borderWidth;
			BorderRadius = borderRadius;
		}

		public MGRectangle Bounds { get; }

		public RenderNode[] Children { get; }

		public MGColor BackgroundColor { get; }

		public MGColor BorderColor { get; }

		public int BorderWidth { get; }

		public BorderRadius BorderRadius { get; }
	}

	internal class GuiRenderer : IDisposable
	{
		private Bitmap _image;
		private Graphics _graphics;

		public GuiRenderer(int width, int height)
		{
			_image = new Bitmap(width, height);
			_graphics = Graphics.FromImage(_image);
			_graphics.CompositingQuality = CompositingQuality.HighQuality;
			_graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
			_graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
			_graphics.SmoothingMode = SmoothingMode.HighQuality;
			_graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
		}

		public void Dispose()
		{
			_graphics.Dispose();
			_image.Dispose();
		}

		public void Render(RenderTree tree, byte[] data)
		{
			_graphics.Clear(Color.Transparent);
			RenderNode(_graphics, tree.Root);
			_graphics.Save();
			var bd = _image.LockBits(new Rectangle(0, 0, _image.Width, _image.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			Marshal.Copy(bd.Scan0, data, 0, data.Length);
			byte swap;
			for (int i = 0; i < data.Length; i += 4)
			{
				swap = data[i + 2];
				data[i + 2] = data[i];
				data[i] = swap;
			}
			_image.UnlockBits(bd);
		}

		private static void RenderNode(Graphics graphics, RenderNode node)
		{
			node.Match(
				text: t => RenderText(graphics, t),
				box: b => RenderBox(graphics, b)
			);
		}

		private static void RenderText(Graphics graphics, Text node)
		{
		}

		private static void RenderBox(Graphics graphics, Box box)
		{
			using (var pen = new Pen(box.BorderColor.ToSys(), box.BorderWidth))
			using (var path = MakeBoxPath(box))
			using (var bgBrush = new SolidBrush(box.BackgroundColor.ToSys()))
			{
				graphics.FillPath(bgBrush, path);
				graphics.DrawPath(pen, path);
			}
		}

		// Adapted from: http://csharphelper.com/blog/2016/01/draw-rounded-rectangles-in-c/
		private static GraphicsPath MakeBoxPath(Box box)
		{
			PointF point1, point2;
			var path = new GraphicsPath();

			var rectangle = box.Bounds;

			if (box.BorderRadius.TopLeft > 0)
			{
				var radius = box.BorderRadius.TopLeft;
				var corner = new RectangleF(rectangle.X, rectangle.Y, 2 * radius, 2 * radius);
				path.AddArc(corner, 180, 90);
				point1 = new PointF(rectangle.X + radius, rectangle.Y);
			}
			else
			{
				point1 = new PointF(rectangle.X, rectangle.Y);
			}

			if (box.BorderRadius.TopRight > 0)
			{
				var radius = box.BorderRadius.TopRight;
				point2 = new PointF(rectangle.Right - radius, rectangle.Y);
			}
			else
			{
				point2 = new PointF(rectangle.Right, rectangle.Y);
			}
			path.AddLine(point1, point2);

			if (box.BorderRadius.TopRight > 0)
			{
				var radius = box.BorderRadius.TopRight;
				var corner = new RectangleF(rectangle.Right - 2 * radius, rectangle.Y, 2 * radius, 2 * radius);
				path.AddArc(corner, 270, 90);
				point1 = new PointF(rectangle.Right, rectangle.Y + radius);
			}
			else
			{
				point1 = new PointF(rectangle.Right, rectangle.Y);
			}

			if (box.BorderRadius.BottomRight > 0)
			{
				var radius = box.BorderRadius.BottomRight;
				point2 = new PointF(rectangle.Right, rectangle.Bottom - radius);
			}
			else
			{
				point2 = new PointF(rectangle.Right, rectangle.Bottom);
			}
			path.AddLine(point1, point2);

			if (box.BorderRadius.BottomRight > 0)
			{
				var radius = box.BorderRadius.BottomRight;
				var corner = new RectangleF(rectangle.Right - 2 * radius, rectangle.Bottom - 2 * radius, 2 * radius, 2 * radius);
				path.AddArc(corner, 0, 90);
				point1 = new PointF(rectangle.Right - radius, rectangle.Bottom);
			}
			else
			{
				point1 = new PointF(rectangle.Right, rectangle.Bottom);
			}

			if (box.BorderRadius.BottomLeft > 0)
			{
				var radius = box.BorderRadius.BottomLeft;
				point2 = new PointF(rectangle.X + radius, rectangle.Bottom);
			}
			else
			{
				point2 = new PointF(rectangle.X, rectangle.Bottom);
			}
			path.AddLine(point1, point2);

			// Lower left corner.
			if (box.BorderRadius.BottomLeft > 0)
			{
				var radius = box.BorderRadius.BottomLeft;
				var corner = new RectangleF(rectangle.X, rectangle.Bottom - 2 * radius, 2 * radius, 2 * radius);
				path.AddArc(corner, 90, 90);
				point1 = new PointF(rectangle.X, rectangle.Bottom - radius);
			}
			else
			{
				point1 = new PointF(rectangle.X, rectangle.Bottom);
			}

			if (box.BorderRadius.TopLeft > 0)
			{
				var radius = box.BorderRadius.TopLeft;
				point2 = new PointF(rectangle.X, rectangle.Y + radius);
			}
			else
			{
				point2 = new PointF(rectangle.X, rectangle.Y);
			}
			path.AddLine(point1, point2);

			path.CloseFigure();
			return path;
		}
	}

	internal static class SysExt
	{
		internal static Color ToSys(this MGColor self) => Color.FromArgb(self.A, self.R, self.G, self.B);
		internal static Rectangle ToSys(this MGRectangle self) => new Rectangle(self.X, self.Y, self.Width, self.Height);
	}
}
