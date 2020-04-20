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
				case nameof(MainMenu): return MainMenu.Factory.Create(gameState as TowerDefenseGameState);
				case nameof(GameOver): return GameOver.Factory.Create(gameState as TowerDefenseGameState);
				case nameof(Gameplay): return Gameplay.Factory.Create(gameState as TowerDefenseGameState);
				default: throw new NotImplementedException("TODO");
			}
		}
	}
}
