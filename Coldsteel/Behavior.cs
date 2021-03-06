﻿// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Coldsteel.Controls;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Coldsteel
{
	public abstract class Behavior : Component
	{
		private List<Coroutine> _pendingCoroutines = new List<Coroutine>();

		private List<Coroutine> _coroutines = new List<Coroutine>();

		protected GameTime GameTime;

		protected float Delta;

		private protected override void Activated()
		{
			Engine.BehaviorSystem.AddComponent(Scene, this);
			Initialize();
		}

		private protected override void OnReady()
		{
			Start();
		}

		private protected override void Deactivated()
		{
			Engine.BehaviorSystem.RemoveComponent(Scene, this);
		}

		internal void Update(GameTime gameTime)
		{
			if (Dead) return;
			GameTime = gameTime;
			Delta = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
			Update();
		}

		protected virtual void Initialize() { }

		protected virtual void Start() { }

		internal protected virtual void HandleCollision(Collision collision) { }

		protected virtual void Update() { }

		public Control GetControl(string name)
		{
			if (!Engine.Config.Controls.ContainsKey(name))
				throw new System.Exception($"control not defined: {name}");

			return Engine.Config.Controls[name];
		}

		public TControl GetControl<TControl>(string name) where TControl : Control
		{
			if (!Engine.Config.Controls.ContainsKey(name))
				throw new System.Exception($"control not defined: {name}");

			return Engine.Config.Controls[name] as TControl;
		}

		public Vector2 ScreenToView(Vector2 target)
		{
			return Engine.RenderingSystem.PointToScreen(target.ToPoint()).ToVector2();
		}

		protected Coroutine StartCoroutine(IEnumerator routine)
		{
			var coroutine = new Coroutine(routine);
			_pendingCoroutines.Add(coroutine);
			return coroutine;
		}

		internal void UpdateCoroutines(GameTime gameTime)
		{
			_coroutines.RemoveAll(c => c.IsFinished);
			_coroutines.AddRange(_pendingCoroutines);
			_pendingCoroutines.Clear();
			_coroutines.ForEach(c => c.Update(gameTime));
		}

		public void PlaySong(string name, bool loop = false)
		{
			Engine.AudioSystem.PlaySong(name, loop);
		}

		public void StopSong()
		{
			Engine.AudioSystem.StopSong();
		}
	}
}
