using Coldsteel;

namespace LD46
{
	public static class SpriteLayers
	{
		public const string BottomMost = "BottomMost";
		public const string Default = "Default";
		public const string Topmost = "Topmost";

		internal static void AddSpriteLayers(this Scene scene)
		{
			scene
				.AddSpriteLayer(BottomMost, -100)
				.AddSpriteLayer(Default, 0)
				.AddSpriteLayer(Topmost, 100);
		}
	}
}
