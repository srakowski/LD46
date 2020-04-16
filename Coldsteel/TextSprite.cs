// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Coldsteel
{
	public class TextSprite : Component, IRenderer
	{
		private Asset<SpriteFont> _spriteFont;

		public string AssetName;

		public Rectangle? SourceRectangle;

		public Color Color = Color.White;

		public Vector2 Origin;

		public SpriteEffects SpriteEffects;

		public float LayerDepth;

		public bool Enabled = true;

		public string RenderingLayerName;

		public string Text = "";

		bool IRenderer.Enabled => Enabled && !Dead;

		string IRenderer.RenderingLayerName => RenderingLayerName;

		public TextSprite() { }

		public TextSprite(string assetName, string text, string renderingLayerName)
		{
			AssetName = assetName;
			Text = text;
			RenderingLayerName = renderingLayerName;
		}

		private protected override void Activated()
		{
			Engine.RenderingSystem.AddRenderer(Scene, this);
			_spriteFont = Scene.Assets.FirstOrDefault(a => a.Name == AssetName) as Asset<SpriteFont>;
		}

		private protected override void Deactivated()
		{
			_spriteFont = null;
			Engine.RenderingSystem.RemoveRenderer(Scene, this);
		}

		internal void Draw(SpriteBatch spriteBatch)
		{
			if (!(_spriteFont?.IsLoaded ?? false) || !Enabled) return;
			spriteBatch.DrawString(
				_spriteFont.GetValue(),
				Text,
				Entity.GlobalPosition,
				Color,
				Entity.GlobalRotation,
				Origin,
				Entity.GlobalScale,
				SpriteEffects,
				LayerDepth
			);
		}

		void IRenderer.Draw(SpriteBatch spriteBatch) => Draw(spriteBatch);
	}
}