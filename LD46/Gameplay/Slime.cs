using Coldsteel;
using Coldsteel.Animations;
using Microsoft.Xna.Framework;
using Roy_T.AStar.Grids;
using Roy_T.AStar.Paths;
using Roy_T.AStar.Primitives;
using System.Collections;
using System.Linq;
using static LD46.Gameplay.Level;
using CSSize = Coldsteel.Size;

namespace LD46.Gameplay
{
	class Slime : Entity
	{
		private Level level;
		private SlimeBehavior behavior;
		private int HP;

		public Slime(Level level, float depth, int rank)
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

			behavior = new SlimeBehavior(this, level).AddToEntity(this);

			HP = (int)((rank + 1) * Settings.HPMultiplier);
		}

		public Tile Target => behavior.Target;

		internal void Hit(int dmg)
		{
			HP -= dmg;
			if (HP <= 0)
			{
				Dead = true;
				level.AddGold(10);
			}
		}

		private class SlimeBehavior : Behavior
		{
			private Slime slime;
			private Level level;
			public Level.Tile Target;

			public SlimeBehavior(Slime slime, Level level)
			{
				this.slime = slime;
				this.level = level;
			}

			protected override void Start()
			{
				this.Target = level.SlimeActivationTile;
				StartCoroutine(MoveToTarget());
			}

			private IEnumerator MoveToTarget()
			{
				while (true)
				{
					while (Vector2.Distance(this.Entity.Position, Target.Position) > 0.5f)
					{
						var direction = Target.Position - this.Entity.Position;
						direction.Normalize();
						var velocity = direction * Settings.SlimeSpeed;
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
					Target = nextTarget;
				}
			}

			private Tile FindPath()
			{
				Grid grid = level.GetGrid();

				var pathFinder = new PathFinder();
				var path = pathFinder.FindPath(
					new GridPosition(Target.MapPosition.X, Target.MapPosition.Y),
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
