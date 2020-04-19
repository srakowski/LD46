using Microsoft.Xna.Framework;
using System;

namespace Coldsteel
{
	public static class Vector2Helper
	{
		public static Vector2 FromAngle(float radians) => new Vector2((float)Math.Cos(radians), (float)Math.Sin(radians));
		public static float ToAngle(this Vector2 vector) => (float)Math.Atan2(vector.X, -vector.Y);
	}
}
