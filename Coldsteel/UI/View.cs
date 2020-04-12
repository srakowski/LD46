// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Coldsteel.UI.Elements;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coldsteel.UI
{
	public class View : Component, IElementCollection
	{
		private List<Element> _elements = new List<Element>();

		public IEnumerable<Element> Elements
		{
			get => _elements;
			set => _elements = value.ToList();
		}

		public Color BackgroundColor { get; set; } = Color.Transparent;

		public View AddElement(Element element)
		{
			_elements.Add(element);
			return this;
		}

		private protected override void Activated()
		{
			Engine.UISystem.AddView(Scene, this);
		}

		private protected override void Deactivated()
		{
			Engine.UISystem.RemoveView(Scene, this);
		}

		internal void HandleMouseClick(Point position)
		{
			var eventArgs = new MouseClickEventArgs(position);
			HandleMouseClick(position, this, eventArgs);
		}

		private static void HandleMouseClick(Point position, IElementCollection ec, MouseClickEventArgs e)
		{
			var element = ec.Elements.FirstOrDefault(ev => ev.Bounds.Contains(position));
			if (element == null) return;
			if (element is Div div)
			{
				HandleMouseClick(position, div, e);
				if (!e.Handled)
				{
					div.MouseClick(e);
				}
			}
		}

		internal void HandleMouseMovement(Point position)
		{
			var eventArgs = new MouseMovementEventArgs(position);
			HandleMouseMovement(this, eventArgs);
		}

		private static void HandleMouseMovement(IElementCollection elementCollection, MouseMovementEventArgs e)
		{
			foreach (var element in elementCollection.Elements)
			{
				if (element is Div div)
				{
					HandleMouseMovement(div, e);
					div.HandleMouseMove(e);
				}
			}
		}

		internal void UpdateLayout(Rectangle bounds) => UpdateLayout(bounds, this);

		private static void UpdateLayout(Rectangle bounds, IElementCollection elementCollection)
		{
			var anchorPoints = new AnchorPoints(bounds);
			foreach (var element in elementCollection.Elements)
			{
				element.UpdateBounds(bounds, anchorPoints);
				if (element is IElementCollection ec)
				{
					UpdateLayout(element.Bounds, ec);
				}
			}
		}

		internal void Render(GuiRenderer guiRenderer, byte[] bytes)
		{
			guiRenderer.Clear(BackgroundColor);
			Render(guiRenderer, this);
			guiRenderer.Export(bytes);
		}

		private static void Render(GuiRenderer guiRenderer, IElementCollection elementCollection)
		{
			foreach (var element in elementCollection.Elements)
			{
				if (element is Elements.Text t)
				{
					guiRenderer.RenderText(t);
				}
				else if (element is Elements.Div d)
				{
					guiRenderer.RenderDiv(d);
					Render(guiRenderer, d);
				}
				else  if (element is Elements.Image i)
				{
					guiRenderer.RenderImage(i);
				}
				else
				{
					throw new NotImplementedException();
				}
			}
		}
	}
}
