// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework;

namespace Coldsteel
{
	internal class SceneManager : GameComponent
	{
		private Scene _pendingScene;
		private readonly Engine _engine;

		public SceneManager(Game game, Engine engine, ISceneFactory sceneFactory) : base(game)
		{
			_engine = engine;
			SceneFactory = sceneFactory;
			game.Components.Add(this);
		}

		public Scene ActiveScene { get; private set; }

		public ISceneFactory SceneFactory { get; internal set; }

		internal void LoadScene(string sceneName, GameState gameState)
		{
			var scene = SceneFactory.Create(sceneName, gameState);
			_pendingScene = scene;
		}

		public override void Update(GameTime gameTime)
		{
			if (_pendingScene == null) return;

			ActiveScene?.Deactivate();
			_pendingScene.Activate(_engine);
			ActiveScene = _pendingScene;
			_pendingScene = null;
		}
	}
}
