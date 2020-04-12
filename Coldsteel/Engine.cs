// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Coldsteel.Particles;
using Coldsteel.UI;
using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("Coldsteel.DevTools")]
[assembly:InternalsVisibleTo("Coldsteel.ParticleTool")]

namespace Coldsteel
{
	public class Engine : GameComponent
	{
		public Engine(Game game, GraphicsDeviceManager graphics, EngineConfig config) : base(game)
		{
			Config = config;
			game.Services.AddService<GraphicsDeviceManager>(graphics);
			game.Components.Add(this);
			SceneManager = new SceneManager(game, this, config.SceneFactory);
			InputManager = new InputManager(game, this);
			BehaviorSystem = new BehaviorSystem(game, this);
			CollisionSystem = new CollisionSystem(game, this);
			ParticleSystem = new ParticleSystem(game, this);
			UISystem = new UISystem(game, this);
			RenderingSystem = new RenderingSystem(game, this);
		}

		internal EngineConfig Config;

		internal SceneManager SceneManager;

		internal InputManager InputManager;

		internal BehaviorSystem BehaviorSystem;

		internal CollisionSystem CollisionSystem;

		internal ParticleSystem ParticleSystem;

		internal UISystem UISystem;

		internal RenderingSystem RenderingSystem;

		public void LoadScene(string sceneName, GameState gameState)
		{
			SceneManager.LoadScene(sceneName, gameState);
		}
	}
}
