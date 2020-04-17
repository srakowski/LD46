using Coldsteel;

namespace LD46.Gameplay
{
	static class Factory
	{
		public static Scene Create(LDJamGameState gameState)
		{
			var scene = new Scene();
			scene.AddAssetsFromDirectory(@"./Content");
			return scene;
		}
	}
}
