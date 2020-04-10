using Coldsteel;
using System;

namespace LD46
{
	class SceneFactory : ISceneFactory
	{
		public Scene Create(string sceneName, GameState gameState)
		{
			switch (sceneName)
			{
				case nameof(GameplayScene): return GameplayScene(gameState as LDJamGameState);
				default: throw new NotImplementedException("TODO");
			}
		}

		public Scene GameplayScene(LDJamGameState gameState)
		{
			var scene = new Scene();

			// TODO: add assets

			scene.AddSpriteLayers();

			// TODO: add entities

			return scene;
		}
	}
}
