using Microsoft.Xna.Framework.Graphics;

namespace Coldsteel
{
	public interface IRenderer
	{
		string RenderingLayerName { get; }
		bool Enabled { get; }
		void Draw(SpriteBatch spriteBatch);
	}
}
