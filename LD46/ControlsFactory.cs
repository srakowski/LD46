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
		[KeyboardButton(Keys.Q)]
		public ButtonControl BlueTurret { get; private set; }

		[KeyboardButton(Keys.W)]
		public ButtonControl GreenTower { get; private set; }

		[KeyboardButton(Keys.E)]
		public ButtonControl RedTower { get; private set; }

		[KeyboardButton(Keys.R)]
		public ButtonControl BlackTower { get; private set; }


		[KeyboardButton(Keys.Up)]
		public ButtonControl ShootUp { get; private set; }

		[KeyboardButton(Keys.Down)]
		public ButtonControl ShootDown { get; private set; }

		[KeyboardButton(Keys.Left)]
		public ButtonControl ShootLeft { get; private set; }

		[KeyboardButton(Keys.Right)]
		public ButtonControl ShootRight { get; private set; }


		[KeyboardButton(Keys.Z)]
		[KeyboardButton(Keys.Enter)]
		public ButtonControl Action { get; private set; }

		[KeyboardButton(Keys.X)]
		public ButtonControl AltAction { get; private set; }

		[MouseButton(MouseButton.Left)]
		[MouseButton(MouseButton.Right)]
		public ButtonControl Click { get; private set; }

		[MousePositional()]
		public PositionalControl MouseClickLocation { get; private set; }

		[KeyboardButton(Keys.Space)]
		public ButtonControl Fire { get; private set; }

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

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	class MouseButtonAttribute : BindingAttribute
	{
		public MouseButtonAttribute(MouseButton button)
		{
			Button = button;
		}

		public MouseButton Button { get; }

		public override void Bind(Control c)
		{
			var bc = c as ButtonControl;
			if (bc == null) return;
			bc.AddBinding(new MouseButtonControlBinding(Button));
		}
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	class MousePositionalAttribute : BindingAttribute
	{
		public MousePositionalAttribute()
		{
		}

		public override void Bind(Control c)
		{
			var bc = c as PositionalControl;
			if (bc == null) return;
			bc.AddBinding(new MousePositionalControlBinding());
		}
	}
}
