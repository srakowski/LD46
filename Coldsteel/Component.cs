// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Coldsteel
{
	public abstract class Component
	{
		protected Engine Engine { get; private set; }

		protected Scene Scene { get; private set; }

		public Guid Id { get; } = Guid.NewGuid();

		public string Name { get; set; }

		public Entity Entity { get; private set; }

		private protected virtual void Activated() { }

		private protected virtual void Deactivated() { }

		internal void Activate(Engine engine, Scene scene, Entity entity)
		{
			Engine = engine;
			Scene = scene;
			Entity = entity;
			Activated();
		}

		internal void Deactivate()
		{
			Deactivated();
			Entity = null;
			Scene = null;
			Engine = null;
		}
	}

	public static class ComponentExtensions
	{
		public static TComponent AddToEntity<TComponent>(this TComponent component, Entity entity) where TComponent : Component
		{
			entity.AddComponent(component);
			return component;
		}
	}
}