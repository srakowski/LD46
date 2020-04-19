using Coldsteel;
using Microsoft.Xna.Framework;
using Roy_T.AStar.Paths;
using Roy_T.AStar.Primitives;
using System.Collections.Generic;
using System.Linq;

namespace LD46.Gameplay
{
	class TurretPlacer : Behavior
	{
		private Controls controls;
		private Level level;
		private Camera camera;
		private TurretyType turretType = TurretyType.BlueTurret;
		Dictionary<TurretyType, TurretPicker> pickers;
		private Dictionary<TurretyType, Turret> visualTurrets = new Dictionary<TurretyType, Turret>();

		public TurretPlacer(Level level, Camera camera, Dictionary<TurretyType, TurretPicker> pickers)
		{
			this.level = level;
			this.camera = camera;
			this.pickers = pickers;
			this.visualTurrets = new Dictionary<TurretyType, Turret>()
			{
				{TurretyType.BlueTurret,  new Turret(TurretyType.BlueTurret, true) },
				{TurretyType.Green,  new Turret(TurretyType.Green, true) },
				{TurretyType.Red,  new Turret(TurretyType.Red, true) },
				{TurretyType.Dark,  new Turret(TurretyType.Dark, true) },
			};
		}

		protected override void Initialize()
		{
			controls = Controls.Map(this);
			foreach (var p in visualTurrets.Values)
			{
				Scene.AddEntity(p);
			}
		}

		protected override void Update()
		{
			if (controls.BlueTurret.WasPushed())
			{
				turretType = TurretyType.BlueTurret;
			}
			else if (controls.GreenTower.WasPushed())
			{
				turretType = TurretyType.Green;
			}
			else if (controls.RedTower.WasPushed())
			{
				turretType = TurretyType.Red;
			}
			else if (controls.BlackTower.WasPushed())
			{
				turretType = TurretyType.Dark;
			}

			foreach (var p in pickers.Values) p.Selected = false;
			pickers[turretType].Selected = true;

			foreach (var p in visualTurrets.Values)
			{
				p.GetComponent<Sprite>().Enabled = false;
			}

			var target = controls.MouseClickLocation.GetPosition();
			var clickPosition = camera.ToWorldCoords(ScreenToView(target)) + new Vector2(8, 8);
			var mapPos = (clickPosition / new Vector2(16f, 16f)).ToPoint();
			var tile = level.GetTileAt(mapPos);

			var tileTargetable = tile != null && tile.IsTraversable && !(tile.occupant is TargetSlime) && !Scene.Entities.OfType<Slime>().Any(s => s.Target == tile);

			if (!tileTargetable) return;

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

			visualTurrets[turretType].GetComponent<Sprite>().Enabled = true;
			visualTurrets[turretType].Position = tile.Position;

			if (controls.Click.WasPushed())
			{
				var turret = new Turret(turretType);
				Scene.AddEntity(turret);
				tile.SetOccupant(turret);
			}
		}
	}
}
