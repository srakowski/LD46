using Coldsteel;

namespace LD46
{
	public static class RenderingLayers
	{
		public const string BottomMost = "BottomMost";
		public const string Default = "Default";
		public const string Topmost = "Topmost";

		internal static void AddRenderingLayers(this Scene scene)
		{
			scene
				.AddRenderingLayer(BottomMost, -100)
				.AddRenderingLayer(Default, 0)
				.AddRenderingLayer(Topmost, 100);
		}
	}
}
