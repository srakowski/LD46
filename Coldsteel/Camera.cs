// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework;

namespace Coldsteel
{
	public class Camera : Component
	{
		private bool _enabled = true;

		public bool Enabled
		{
			get => _enabled && !Dead;
			set => _enabled = value;
		}

		public Vector2 ToWorldCoords(Vector2 coords) =>
			Vector2.Transform(coords, Matrix.Invert(TransformationMatrix));

		internal Matrix TransformationMatrix =>
			Matrix.Identity *
			Matrix.CreateRotationZ(Entity.GlobalRotation) *
			Matrix.CreateScale(Entity.GlobalScale) *
			Matrix.CreateTranslation(-Entity.GlobalPosition.X, -Entity.GlobalPosition.Y, 0f) *
			Matrix.CreateTranslation(
				(Engine.Config.ScreenDim.Width * 0.5f),
				(Engine.Config.ScreenDim.Height * 0.5f),
				0f);

		private protected override void Activated()
		{
			Engine.RenderingSystem.AddCamera(Scene, this);
		}

		private protected override void Deactivated()
		{
			Engine.RenderingSystem.RemoveCamera(Scene, this);
		}
	}
}
