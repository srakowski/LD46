// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coldsteel.Animations
{
	public class SpriteAnimator : Component
	{
		private readonly List<SpriteAnimation> _spriteAnimations = new List<SpriteAnimation>();
		private readonly Sprite _sprite;

		private SpriteAnimation _currentAnimation;
		private int _currentFrameIndex;
		private double _timeRemainingThisFrame;

		public SpriteAnimator(Sprite sprite)
		{
			_sprite = sprite;
		}

		public string CurrentAnimationName => _currentAnimation.Name;

		public SpriteAnimator AddSpriteAnimation(SpriteAnimation spriteAnimation)
		{
			_spriteAnimations.Add(spriteAnimation);
			return this;
		}

		public SpriteAnimator AddSpriteAnimation(string name, params Frame[] frames) =>
			AddSpriteAnimation(new SpriteAnimation(name, frames));

		public SpriteAnimator AddSpriteAnimation(string name, params (int Index, int Duration)[] frames) =>
			AddSpriteAnimation(name, frames.Select(l => new Frame(l.Index, l.Duration)).ToArray());

		private protected override void Activated()
		{
			Engine.AnimationSystem.AddComponent(Scene, this);
		}

		private protected override void Deactivated()
		{
			Engine.AnimationSystem.RemoveComponent(Scene, this);
		}

		internal void Update(GameTime gameTime)
		{
			if (_currentAnimation == null) LoadAnimation(_spriteAnimations.First());
			if (_currentAnimation == null) return;

			_timeRemainingThisFrame -= gameTime.ElapsedGameTime.TotalMilliseconds;
			if (_timeRemainingThisFrame > 0) return;

			_currentFrameIndex += 1;
			LoadFrame();
		}

		private void LoadAnimation(SpriteAnimation spriteAnimation)
		{
			_currentAnimation = spriteAnimation;
			_currentFrameIndex = 0;
			LoadFrame();
		}

		public void Animate(string name) => LoadAnimation(_spriteAnimations.First(a => a.Name == name));

		private void LoadFrame()
		{
			var frame = GetFrame();
			_timeRemainingThisFrame = frame.Duration;
			_sprite.FrameIndex = frame.Index;
		}

		private Frame GetFrame()
		{
			return _currentAnimation.Frames[_currentFrameIndex % _currentAnimation.Frames.Length];
		}
	}
}