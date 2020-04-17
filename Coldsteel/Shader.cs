// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework.Graphics;

namespace Coldsteel
{
	public abstract class Shader
	{
		private static Asset<Effect> _effectAsset;

		protected Shader(string assetName)
		{
			AssetName = assetName;
		}

		public string AssetName { get; }

		public Effect Effect => _effectAsset.IsLoaded ? _effectAsset.GetValue() : null;

		internal void Activate(Engine engine, Scene scene)
		{
			_effectAsset = scene.GetAsset<Effect>(AssetName);
		}

		internal void Deactivate()
		{
			_effectAsset = null;
		}

		public virtual void ApplyParameters() { }
	}
}
