// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Coldsteel
{
	internal abstract class SystemBase<TComponent> : GameComponent where TComponent : Component
	{
		private Dictionary<Scene, List<TComponent>> _componentsByScene = new Dictionary<Scene, List<TComponent>>();

		protected readonly Engine Engine;

		protected List<TComponent> ActiveComponents =>
			_componentsByScene.TryGetValue(Engine.SceneManager.ActiveScene, out var components)
				? components
				: null;

		public SystemBase(Game game, Engine engine) : base(game)
		{
			Engine = engine;
			game.Components.Add(this);
		}

		public void AddComponent(Scene scene, TComponent component)
		{
			var components = GetComponentsByScene(scene);
			components.Add(component);
		}

		public void RemoveComponent(Scene scene, TComponent component)
		{
			var components = GetComponentsByScene(scene);
			components.Remove(component);
		}

		private List<TComponent> GetComponentsByScene(Scene scene)
		{
			return _componentsByScene.ContainsKey(scene)
				? _componentsByScene[scene]
				: (_componentsByScene[scene] = new List<TComponent>());
		}
	}
}
