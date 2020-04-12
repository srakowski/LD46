// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework;
using System.Collections;
using System.Collections.Generic;

namespace Coldsteel.UI
{
	public class View : Component, IElementCollection
	{
		private readonly ICollection<Element> _elements = new List<Element>();

		public void Add(Element element)
		{
			_elements.Add(element);
		}

		private protected override void Activated()
		{
			Engine.UISystem.AddView(Scene, this);
		}

		private protected override void Deactivated()
		{
			Engine.UISystem.RemoveView(Scene, this);
		}

		internal void UpdateLayout(Rectangle bounds) => UpdateLayout(bounds, this);

		private static void UpdateLayout(Rectangle bounds, IElementCollection elementCollection)
		{
			var anchorPoints = new AnchorPoints(bounds);
			foreach (var element in elementCollection)
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
			guiRenderer.Clear();
			Render(guiRenderer, this);
			guiRenderer.Export(bytes);
		}

		private static void Render(GuiRenderer guiRenderer, IElementCollection elementCollection)
		{
			foreach (var element in elementCollection)
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
			}
		}

		public IEnumerator<Element> GetEnumerator() => _elements.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => _elements.GetEnumerator();
	}
}
