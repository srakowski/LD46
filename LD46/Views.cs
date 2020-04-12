using Coldsteel.UI;
using Coldsteel.UI.Elements;
using Microsoft.Xna.Framework;

namespace LD46
{
	static class Views
	{
		public static View MainMenu => new View
		{
			Div.New
				.Configure(div =>
				{
					div.Anchor = Anchor.TopCenter;
					div.BackgroundColor = Color.Gray;
					div.BorderRadius = 2;
					div.BorderColor = Color.Black;
					div.Offset = new Point(-100, 0);
					div.Height = 60;
					div.Width = 200;
					div.BorderWidth = 1;
				})
				.Add(
					Text.New
						.Configure(text =>
						{
							text.Value = "Hello World!";
							text.Dock = Dock.Fill;
							text.Align = Align.Center;
							text.VerticalAlign = Align.Center;
						})
				),
		};
    }
}
