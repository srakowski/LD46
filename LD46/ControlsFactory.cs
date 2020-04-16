using Coldsteel;
using Coldsteel.Controls;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LD46
{
	class Controls
	{
		[KeyboardButton(Keys.Up)]
		public ButtonControl Up { get; private set; }

		[KeyboardButton(Keys.Down)]
		public ButtonControl Down { get; private set; }

		[KeyboardButton(Keys.Left)]
		public ButtonControl Left { get; private set; }

		[KeyboardButton(Keys.Right)]
		public ButtonControl Right { get; private set; }

		[KeyboardButton(Keys.Z)]
		[KeyboardButton(Keys.Enter)]
		public ButtonControl Action { get; private set; }

		[KeyboardButton(Keys.X)]
		public ButtonControl AltAction { get; private set; }

		public static Controls Map(Behavior behavior)
		{
			var c = new Controls();
			typeof(Controls).GetProperties()
				.Where(p => typeof(Control).IsAssignableFrom(p.PropertyType))
				.ToList()
				.ForEach(p =>
				{
					p.SetValue(c, behavior.GetControl(p.Name));
				});
			return c;
		}
	}

	static class ControlsFactory
	{
		public static IEnumerable<Control> Create()
		{
			return typeof(Controls).GetProperties()
				.Where(p => typeof(Control).IsAssignableFrom(p.PropertyType))
				.Select(p =>
				{
					var control = Activator.CreateInstance(p.PropertyType, p.Name) as Control;
					p.GetCustomAttributes(inherit: true)
						.OfType<BindingAttribute>()
						.ToList()
						.ForEach(ba => ba.Bind(control));
					return control;
				});
		}
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	abstract class BindingAttribute : Attribute
	{
		public abstract void Bind(Control c);
	}

	class KeyboardButtonAttribute : BindingAttribute
	{
		public KeyboardButtonAttribute(Keys key)
		{
			Key = key;
		}

		public Keys Key { get; }

		public override void Bind(Control c)
		{
			var bc = c as ButtonControl;
			if (bc == null) return;
			bc.AddBinding(new KeyboardButtonControlBinding(Key));
		}
	}
}
