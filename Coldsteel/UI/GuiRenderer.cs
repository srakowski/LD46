// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Coldsteel.UI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using CSImage = Coldsteel.UI.Elements.Image;
using Image = System.Drawing.Image;
using MGColor = Microsoft.Xna.Framework.Color;
using MGRectangle = Microsoft.Xna.Framework.Rectangle;

namespace Coldsteel.UI
{
	internal class GuiRenderer : IDisposable
	{
		private Bitmap _image;
		private Graphics _graphics;
		private readonly Dictionary<string, Image> _loadedImages = new Dictionary<string, Image>();

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

		public void Clear(MGColor color)
		{
			_graphics.Clear(color.ToSys());
		}

		internal void Export(byte[] data)
		{
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

		internal void RenderText(Text text)
		{
			var style = FontStyle.Regular;
			if (text.Bold) style |= FontStyle.Bold;
			if (text.Italic) style |= FontStyle.Italic;

			using (var brush = new SolidBrush(text.Color.ToSys()))
			using (var font = new Font(text.Font, text.Size, style))
			{
				var gs = _graphics.Save();
				_graphics.Clip = new Region(text.Bounds.ToSys());
				_graphics.DrawString(text.Value, font, brush, text.Bounds.ToSys(), new StringFormat
				{
					Alignment = text.Align.ToSys(),
					LineAlignment = text.VerticalAlign.ToSys(),
				});
				_graphics.Restore(gs);
			}
		}

		internal void RenderDiv(Div div)
		{
			using (var path = MakeDivPath(div))
			using (var bgBrush = new SolidBrush(div.BackgroundColor.ToSys()))
			{
				var gs = _graphics.Save();
				_graphics.FillPath(bgBrush, path);
				if (div.BorderWidth > 0)
				{
					using (var pen = new Pen(div.BorderColor.ToSys(), div.BorderWidth))
						_graphics.DrawPath(pen, path);
				}
				_graphics.Restore(gs);
			}
		}

		internal void RenderImage(CSImage image)
		{
			if (!_loadedImages.TryGetValue(image.Source, out var img))
			{
				img = Image.FromFile(image.Source);
				_loadedImages[image.Source] = img;
			}
			var gs = _graphics.Save();
			_graphics.DrawImage(img, image.Bounds.ToSys());
			_graphics.Restore(gs);
		}

		// Adapted from: http://csharphelper.com/blog/2016/01/draw-rounded-rectangles-in-c/
		private static GraphicsPath MakeDivPath(Div div)
		{
			PointF point1, point2;
			var path = new GraphicsPath();

			var rectangle = div.Bounds;

			if (div.BorderRadius.TopLeft > 0)
			{
				var radius = div.BorderRadius.TopLeft;
				var corner = new RectangleF(rectangle.X, rectangle.Y, 2 * radius, 2 * radius);
				path.AddArc(corner, 180, 90);
				point1 = new PointF(rectangle.X + radius, rectangle.Y);
			}
			else
			{
				point1 = new PointF(rectangle.X, rectangle.Y);
			}

			if (div.BorderRadius.TopRight > 0)
			{
				var radius = div.BorderRadius.TopRight;
				point2 = new PointF(rectangle.Right - radius, rectangle.Y);
			}
			else
			{
				point2 = new PointF(rectangle.Right, rectangle.Y);
			}
			path.AddLine(point1, point2);

			if (div.BorderRadius.TopRight > 0)
			{
				var radius = div.BorderRadius.TopRight;
				var corner = new RectangleF(rectangle.Right - 2 * radius, rectangle.Y, 2 * radius, 2 * radius);
				path.AddArc(corner, 270, 90);
				point1 = new PointF(rectangle.Right, rectangle.Y + radius);
			}
			else
			{
				point1 = new PointF(rectangle.Right, rectangle.Y);
			}

			if (div.BorderRadius.BottomRight > 0)
			{
				var radius = div.BorderRadius.BottomRight;
				point2 = new PointF(rectangle.Right, rectangle.Bottom - radius);
			}
			else
			{
				point2 = new PointF(rectangle.Right, rectangle.Bottom);
			}
			path.AddLine(point1, point2);

			if (div.BorderRadius.BottomRight > 0)
			{
				var radius = div.BorderRadius.BottomRight;
				var corner = new RectangleF(rectangle.Right - 2 * radius, rectangle.Bottom - 2 * radius, 2 * radius, 2 * radius);
				path.AddArc(corner, 0, 90);
				point1 = new PointF(rectangle.Right - radius, rectangle.Bottom);
			}
			else
			{
				point1 = new PointF(rectangle.Right, rectangle.Bottom);
			}

			if (div.BorderRadius.BottomLeft > 0)
			{
				var radius = div.BorderRadius.BottomLeft;
				point2 = new PointF(rectangle.X + radius, rectangle.Bottom);
			}
			else
			{
				point2 = new PointF(rectangle.X, rectangle.Bottom);
			}
			path.AddLine(point1, point2);

			// Lower left corner.
			if (div.BorderRadius.BottomLeft > 0)
			{
				var radius = div.BorderRadius.BottomLeft;
				var corner = new RectangleF(rectangle.X, rectangle.Bottom - 2 * radius, 2 * radius, 2 * radius);
				path.AddArc(corner, 90, 90);
				point1 = new PointF(rectangle.X, rectangle.Bottom - radius);
			}
			else
			{
				point1 = new PointF(rectangle.X, rectangle.Bottom);
			}

			if (div.BorderRadius.TopLeft > 0)
			{
				var radius = div.BorderRadius.TopLeft;
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
		internal static StringAlignment ToSys(this Align self)
		{
			switch (self)
			{
				case Align.Near: return StringAlignment.Near;
				case Align.Far: return StringAlignment.Far;
				case Align.Center:
				default:
					return StringAlignment.Center;
			}
		}
	}
}
