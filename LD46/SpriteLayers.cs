using Coldsteel;

namespace LD46
{
	public static class SpriteLayers
	{
		public const string Default = "Default";
		public const string Topmost = "Topmost";

		internal static void AddSpriteLayers(this Scene scene)
		{
			scene
				.AddSpriteLayer(Default, 0)
				.AddSpriteLayer(Topmost, 100);
		}
	}
}
