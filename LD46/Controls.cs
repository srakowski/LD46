using Coldsteel.Controls;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace LD46
{
	static class Controls
	{
		public const string PlayerUp = "PlayerUp";
		public const string PlayerDown = "PlayerDown";
		public const string PlayerLeft = "PlayerLeft";
		public const string PlayerRight = "PlayerRight";

		public static IEnumerable<Control> Create() => Create(
			Control.Button(PlayerUp).BindToKeyboard(Keys.Up),
			Control.Button(PlayerDown).BindToKeyboard(Keys.Down),
			Control.Button(PlayerLeft).BindToKeyboard(Keys.Left),
			Control.Button(PlayerRight).BindToKeyboard(Keys.Right)
		);

		private static IEnumerable<Control> Create(params Control[] controls) => controls;
	}
}
