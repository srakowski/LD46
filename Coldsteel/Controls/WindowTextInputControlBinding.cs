// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework;

namespace Coldsteel.Controls
{
	public class WindowTextInputControlBinding : TextInputControlBinding
	{
		public WindowTextInputControlBinding(PlayerIndex playerIndex) : base(playerIndex)
		{
		}

		public override string InputBuffer => InputManager.GetTextInput(this);

		public override void BeginInput() => InputManager.BeginTextInput(this);

		public override void EndInput() => InputManager.EndTextInput(this);
	}
}
