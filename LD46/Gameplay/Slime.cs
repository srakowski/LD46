using Coldsteel;
using Coldsteel.Animations;
using Microsoft.Xna.Framework;
using Roy_T.AStar.Primitives;
using System;
using System.Collections;
using System.Linq;
using ToySize = Roy_T.AStar.Primitives.Size;
using CSSize = Coldsteel.Size;
using Roy_T.AStar.Grids;
using static LD46.Gameplay.Level;
using Roy_T.AStar.Paths;

namespace LD46.Gameplay
{
	class Slime : Entity
	{
		private Level level;

		public Slime(Level level, float depth)
		{
			this.level = level;
			var sprite = new Sprite(
				Assets.Texture2D.slime,
				frameSize: new CSSize(16, 16)
			)
			{
				FrameIndex = 0,
				Origin = new Vector2(8, 8),
				LayerDepth = depth,
				RenderingLayerName = "slimes"
			}
			.AddToEntity(this);

			var animator = new SpriteAnimator(sprite)
				.AddSpriteAnimation("idle", (0, 250), (1, 250))
				.AddToEntity(this);

			animator.Animate("idle");

			new SlimeBehavior(this, level).AddToEntity(this);
		}

		private class SlimeBehavior : Behavior
		{
			const float speed = 0.02f;

			private Slime slime;
			private Level level;
			private Level.Tile target;

			public SlimeBehavior(Slime slime, Level level)
			{
				this.slime = slime;
				this.level = level;
			}

			protected override void Start()
			{
				this.target = level.SlimeActivationTile;
				StartCoroutine(MoveToTarget());
			}

			private IEnumerator MoveToTarget()
			{
				while (true)
				{
					while (Vector2.Distance(this.Entity.Position, target.Position) > 0.5f)
					{
						var direction = target.Position - this.Entity.Position;
						direction.Normalize();
						var velocity = direction * speed;
						Entity.Position += velocity * (float)GameTime.ElapsedGameTime.TotalMilliseconds;
						yield return Wait.None();
					}

					var nextTarget = FindPath();
					if (nextTarget == null)
					{
						this.Entity.Dead = true;
						// HIT
						yield break;
					}
					target = nextTarget;
				}
			}

			private Tile FindPath()
			{
				Grid grid = level.GetGrid();

				var pathFinder = new PathFinder();
				var path = pathFinder.FindPath(
					new GridPosition(target.MapPosition.X, target.MapPosition.Y),
					new GridPosition(level.TargetSlimeTile.MapPosition.X, level.TargetSlimeTile.MapPosition.Y),
					grid
				);

				if (!path.Edges.Any()) return null;

				var targetEdge = path.Edges.First();
				var pos = targetEdge.End.Position;
				var mapPos = new Point((int)(pos.X / 16), (int)(pos.Y / 16));
				return level.GetTileAt(mapPos);
			}
		}
	}
}
