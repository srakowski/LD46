﻿// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Coldsteel
{
	public class Sprite : Component, IRenderer
	{
		private readonly string _assetName;

		private Asset<Texture2D> _texture;

		private Rectangle[] _textureFrames;

		private Size? _frameSize;

		public int FrameIndex = 0;

		public Color Color = Color.White;

		public Vector2 Origin;

		public SpriteEffects SpriteEffects;

		public float LayerDepth;

		public bool Enabled = true;

		public string RenderingLayerName;

		bool IRenderer.Enabled => Enabled && !Dead;

		private Rectangle? SourceRectangle => _textureFrames[FrameIndex % _textureFrames.Length];

		string IRenderer.RenderingLayerName => RenderingLayerName;

		public Sprite(string assetName, string renderingLayerName = null, Size? frameSize = null)
		{
			_assetName = assetName;
			RenderingLayerName = renderingLayerName;
			_frameSize = frameSize;
		}

		private protected override void Activated()
		{
			Engine.RenderingSystem.AddRenderer(Scene, this);
			_texture = Scene.Assets.FirstOrDefault(a => a.Name == _assetName) as Asset<Texture2D>;
			CutTextureFrames();
		}

		private protected override void Deactivated()
		{
			_texture = null;
			Engine.RenderingSystem.RemoveRenderer(Scene, this);
		}

		internal void Draw(SpriteBatch spriteBatch)
		{
			if (!(_texture?.IsLoaded ?? false) || !Enabled) return;
			CutTextureFrames();
			spriteBatch.Draw(
				_texture.GetValue(),
				Entity.GlobalPosition,
				SourceRectangle,
				Color,
				Entity.GlobalRotation,
				Origin,
				Entity.GlobalScale,
				SpriteEffects,
				LayerDepth
			);
		}

		private void CutTextureFrames()
		{
			if (_texture == null || !_texture.IsLoaded) return;
			var t = _texture.GetValue();
			var frameSize = _frameSize ?? new Size(t.Width, t.Height);
			var columns = t.Width / frameSize.Width;
			var rows = t.Height / frameSize.Height;
			_textureFrames = new Rectangle[rows * columns];
			var index = 0;
			for (var r = 0; r < rows; r++)
			{
				for (var c = 0; c < columns; c++)
				{

					_textureFrames[index] = new Rectangle(
						x: c * frameSize.Width,
						y: r * frameSize.Height,
						frameSize.Width,
						frameSize.Height
					);
					index++;
				}
			}
		}

		void IRenderer.Draw(SpriteBatch spriteBatch) => Draw(spriteBatch);
	}
}