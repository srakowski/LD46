// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Coldsteel.Controls;
using System;

namespace Coldsteel
{
	public class RelayBehavior : Behavior
	{
		Action<RelayBehavior> onInitialize;
		Action<RelayBehavior> onUpdate;

		public RelayBehavior(Action<RelayBehavior> onInitialize = null, Action<RelayBehavior> onUpdate = null)
		{
			this.onInitialize = onInitialize;
			this.onUpdate = onUpdate;
		}

		protected override void Initialize() => onInitialize?.Invoke(this);

		protected override void Update() => onUpdate?.Invoke(this);
	}
}
