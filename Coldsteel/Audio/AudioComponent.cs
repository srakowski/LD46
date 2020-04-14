// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Coldsteel.Audio
{
	public abstract class AudioComponent : Component
	{
		private protected override void Activated()
		{
			Engine.AudioSystem.AddComponent(Scene, this);
		}

		private protected override void Deactivated()
		{
			Engine.AudioSystem.RemoveComponent(Scene, this);
		}

		internal abstract void Update();
	}
}
