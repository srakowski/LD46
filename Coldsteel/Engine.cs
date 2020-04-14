// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Coldsteel.Animations;
using Coldsteel.Audio;
using Coldsteel.Particles;
using Coldsteel.UI;
using Microsoft.Xna.Framework;

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
			AudioSystem = new AudioSystem(game, this);
			ParticleSystem = new ParticleSystem(game, this);
			UISystem = new UISystem(game, this);
			AnimationSystem = new AnimationSystem(game, this);
			RenderingSystem = new RenderingSystem(game, this);
		}

		internal readonly EngineConfig Config;
		internal readonly SceneManager SceneManager;
		internal readonly InputManager InputManager;
		internal readonly BehaviorSystem BehaviorSystem;
		internal readonly CollisionSystem CollisionSystem;
		internal readonly AudioSystem AudioSystem;
		internal readonly ParticleSystem ParticleSystem;
		internal readonly UISystem UISystem;
		internal readonly AnimationSystem AnimationSystem;
		internal readonly RenderingSystem RenderingSystem;

		public void LoadScene(string sceneName, GameState gameState)
		{
			SceneManager.LoadScene(sceneName, gameState);
		}
	}
}
