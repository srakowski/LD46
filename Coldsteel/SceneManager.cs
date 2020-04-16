// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework;
using System;

namespace Coldsteel
{
	internal class SceneChangingEventArgs : EventArgs
	{
		public SceneChangingEventArgs(Scene fromScene, Scene toScene)
		{
			FromScene = fromScene;
			ToScene = ToScene;
		}

		public Scene FromScene { get; }

		public Scene ToScene { get; }
	}

	internal class SceneManager : GameComponent
	{
		public event EventHandler<SceneChangingEventArgs> OnSceneChanging;

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
			ActiveScene?.Clean();
			if (_pendingScene == null) return;
			OnSceneChanging?.Invoke(this, new SceneChangingEventArgs(ActiveScene, _pendingScene));
			ActiveScene?.Deactivate();
			_pendingScene.Activate(_engine);
			ActiveScene = _pendingScene;
			_pendingScene = null;
		}
	}
}
