// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace Coldsteel
{
	internal class CollisionSystem : SystemBase<Collider>
	{
		public CollisionSystem(Game game, Engine engine) : base(game, engine)
		{
		}

		public override void Update(GameTime gameTime)
		{
			var colliders = ActiveComponents;
			if (colliders == null) return;
			if (!colliders.Any()) return;

			var collidersThisFrame = colliders.ToArray();
			foreach (var collider in collidersThisFrame)
				collider.Update();

			NarrowCollide(collidersThisFrame);
		}

		private void NarrowCollide(Collider[] colliders)
		{
			for (int i = 0; i < colliders.Length; i++)
			{
				for (int j = i + 1; j < colliders.Length; j++)
				{
					var c1 = colliders[i];
					var c2 = colliders[j];

					var result = CheckCollision(c1.Shape, c2.Shape);
					if (!result.CollidersIntersect)
						continue;

					foreach (var behavior in c1.Entity.Components.OfType<Behavior>().ToArray())
						behavior.HandleCollision(new Collision(c1, c2));

					foreach (var behavior in c2.Entity.Components.OfType<Behavior>().ToArray())
						behavior.HandleCollision(new Collision(c2, c1));
				}
			}
		}

		private CollisionResult CheckCollision(Polygon p1, Polygon p2)
		{
			var result = new CollisionResult();
			result.CollidersIntersect = true;

			var minIntervalDistance = float.MaxValue;
			var minIntervalAxis = Vector2.Zero;

			var edges = p1.Edges.Concat(p2.Edges);
			foreach (var edge in edges)
			{
				// The line perpendicular to an edge is our axis.
				var axis = new Vector2(-edge.Y, edge.X);
				axis.Normalize();

				var i1 = Project(p1, axis);
				var i2 = Project(p2, axis);
				var iDistance = Interval.Distance(i1, i2);

				// If we find a gap between any of the verts then no collision has occured.
				if (iDistance > 0f)
				{
					result.CollidersIntersect = false;
					break;
				}

				var absIntervalDistance = Math.Abs(iDistance);
				if (absIntervalDistance < minIntervalDistance)
				{
					minIntervalDistance = absIntervalDistance;
					minIntervalAxis = axis;
				}
			}

			if (result.CollidersIntersect)
			{
				result.MinIntervalDistance = minIntervalDistance;
				result.MinIntervalAxis = minIntervalAxis;
			}

			return result;
		}

		private struct CollisionResult
		{
			public bool CollidersIntersect;
			public float MinIntervalDistance;
			public Vector2 MinIntervalAxis;
		}

		private static Interval Project(Polygon p, Vector2 axis)
		{
			var dp = Vector2.Dot(axis, p.Vertices[0]);
			var interval = new Interval(dp);
			for (var i = 1; i < p.Vertices.Length; i++)
			{
				dp = Vector2.Dot(axis, p.Vertices[i]);
				interval.Min = Math.Min(dp, interval.Min);
				interval.Max = Math.Max(dp, interval.Max);
			}
			return interval;
		}

		private struct Interval
		{
			public float Min;
			public float Max;

			public Interval(float initial)
			{
				Min = initial;
				Max = initial;
			}

			internal static float Distance(Interval i1, Interval i2) =>
				i1.Min < i2.Min
					? i2.Min - i1.Max
					: i1.Min - i2.Max;
		}
	}
}
