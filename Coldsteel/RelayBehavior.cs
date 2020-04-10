// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Coldsteel
{
	public class RelayBehavior : Behavior
	{
		Action onInitialize;
		Action onUpdate;

		public RelayBehavior(Action onInitialize = null, Action onUpdate = null)
		{
			this.onInitialize = onInitialize;
			this.onUpdate = onUpdate;
		}

		protected override void Initialize() => onInitialize?.Invoke();

		protected override void Update() => onUpdate?.Invoke();
	}
}
