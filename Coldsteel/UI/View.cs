using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Coldsteel.UI
{
	public class View : Component
	{
		private RenderTree renderTree = new RenderTree(
			new Box(
				new Rectangle(100, 100, 400, 300),
				Enumerable.Empty<RenderNode>().ToArray(),
				bgColor: Color.LightSlateGray,
				borderColor: Color.DarkGray,
				borderWidth: 1,
				borderRadius: new BorderRadius(16)
			)
		);

		private protected override void Activated()
		{
			Engine.UISystem.AddView(Scene, this);
		}

		private protected override void Deactivated()
		{
			Engine.UISystem.RemoveView(Scene, this);
		}

		internal RenderTree RenderTree => renderTree;
	}
}
