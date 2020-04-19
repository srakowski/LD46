using Coldsteel;
using Microsoft.Xna.Framework;
using Roy_T.AStar.Paths;
using Roy_T.AStar.Primitives;
using System.Linq;

namespace LD46.Gameplay
{
	class PlayerInterface : Entity
	{
	}

	class TurretPlacer : Behavior
	{
		private Controls controls;
		private Level level;
		private Camera camera;

		public TurretPlacer(Level level, Camera camera)
		{
			this.level = level;
			this.camera = camera;
		}

		protected override void Initialize()
		{
			controls = Controls.Map(this);
		}

		protected override void Update()
		{
			if (controls.RightClick.WasPushed())
			{
				var target = controls.MouseClickLocation.GetPosition();
				var clickPosition = camera.ToWorldCoords(ScreenToView(target)) + new Vector2(8, 8);
				var mapPos = (clickPosition / new Vector2(16f, 16f)).ToPoint();
				var tile = level.GetTileAt(mapPos);
				if (tile == null) return;
				if (!tile.IsTraversable) return;
				if (tile.occupant is TargetSlime) return;
				if (Scene.Entities.OfType<Slime>().Any(s => s.Target == tile)) return;

				var grid = level.GetGrid();
				var nodePos = new GridPosition(tile.MapPosition.X, tile.MapPosition.Y);
				grid.DisconnectNode(nodePos);
				grid.RemoveDiagonalConnectionsIntersectingWithNode(nodePos);

				var pathFinder = new PathFinder();
				var path = pathFinder.FindPath(
					new GridPosition(level.SlimeActivationTile.MapPosition.X, level.SlimeActivationTile.MapPosition.Y),
					new GridPosition(level.TargetSlimeTile.MapPosition.X, level.TargetSlimeTile.MapPosition.Y),
					grid
				);

				if (!path.Edges.Any() || path.Type != PathType.Complete)
				{
					// CANT
					return;
				}

				var turret = new Turret();
				Scene.AddEntity(turret);
				tile.SetOccupant(turret);
			}
		}
	}
}
