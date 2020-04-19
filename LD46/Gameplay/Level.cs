using Coldsteel;
using LD46.Content;
using Microsoft.Xna.Framework;
using Roy_T.AStar.Grids;
using Roy_T.AStar.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LD46.Gameplay
{
	class Level : Entity
	{
		private Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();

		public Level(int width, int height)
		{
			Columns = width;
			Rows = height;

			var dim = new Rectangle(0, 0, width, height);
			CenterPoint = dim.Center.ToVector2() * new Vector2(16, 16);

			var rand = new Random();

			for (int y = 0; y < dim.Height; y++)
				for (int x = 0; x < dim.Width; x++)
				{
					var pos = new Point(x, y);
					var tileFrame = ComputeTileFrame(dim.Bottom - 1, dim.Right - 1, rand, y, x);
					var newTile = new Tile(pos, tileFrame, GetIsStatic(dim.Bottom - 1, dim.Right - 1, y, x))
					{
						Rotation = (y == dim.Bottom - 2 && x != 0 && x != dim.Right - 1)
							? MathHelper.ToRadians(180)
							: 0f,
					};
					this.AddChild(newTile);
					tiles[pos] = newTile;
				}

			var targetSlime = new TargetSlime();
			this.AddChild(targetSlime);
			var mapPos = new Point(2, height - 3);
			var tile = tiles[mapPos];
			tile.SetOccupant(targetSlime);
			TargetSlimeTile = tile;
			TargetSlime = targetSlime;

			SlimeSpawner = new SlimeSpawner(this); 
			this.AddChild(SlimeSpawner);
			SlimeSpawner.Position = new Vector2(width, 4) * new Vector2(16, 16);

			SlimeActivationTile = tiles[new Point(width - 2, 4)];
		}

		public IEnumerable<Tile> Tiles => tiles.Values;

		public Vector2 CenterPoint { get; }

		public Tile SlimeActivationTile { get; }

		public TargetSlime TargetSlime { get; }

		public Tile TargetSlimeTile { get; }

		public SlimeSpawner SlimeSpawner { get; }

		public int Columns { get; }
		public int Rows { get; }

		private static int ComputeTileFrame(int b, int r, Random rand, int y, int x)
		{
			var tilesFrame = 0;
			if (y == 0 && x == 0) tilesFrame = TileAtlas.TopLeftEdge;
			else if (y == 0 && x == r) tilesFrame = TileAtlas.TopRightEdge;
			else if (y == b && x == 0) tilesFrame = TileAtlas.BottomLeftEdge;
			else if (y == b && x == r) tilesFrame = TileAtlas.BottomRightEdge;
			else if (y == 0) tilesFrame = TileAtlas.TopRow.OrderBy(_ => rand.Next()).First();
			else if (y == 1 && x != 0 && x != r) tilesFrame = TileAtlas.TopWall.OrderBy(_ => rand.Next()).First();
			else if (y == b - 1 && x != 0 && x != r) tilesFrame = TileAtlas.TopWall.OrderBy(_ => rand.Next()).First();
			else if (y == b) tilesFrame = TileAtlas.BotRow.OrderBy(_ => rand.Next()).First();
			else if (x == 0) tilesFrame = TileAtlas.LeftCol.OrderBy(_ => rand.Next()).First();
			else if (x == r) tilesFrame = TileAtlas.RightCol.OrderBy(_ => rand.Next()).First();
			else tilesFrame = TileAtlas.Floor.OrderBy(_ => rand.Next()).First();
			return tilesFrame;
		}

		internal Grid GetGrid()
		{
			var gridSize = new GridSize(columns: Columns, rows: Rows);
			var cellSize = new Roy_T.AStar.Primitives.Size(Distance.FromMeters(16), Distance.FromMeters(16));
			var traversalVelocity = Velocity.FromKilometersPerHour(100);
			var grid = Grid.CreateGridWithLateralConnections(gridSize, cellSize, traversalVelocity);

			var tiles = Tiles.Where(t => !t.IsTraversable);
			foreach (var tile in tiles)
			{
				var nodePos = new GridPosition(tile.MapPosition.X, tile.MapPosition.Y);
				grid.DisconnectNode(nodePos);
				grid.RemoveDiagonalConnectionsIntersectingWithNode(nodePos);
			}

			return grid;
		}

		internal Tile GetTileAt(Point mapPos) => tiles.TryGetValue(mapPos, out var tile) ? tile : null;

		private static bool GetIsStatic(int b, int r, int y, int x)
		{
			return (y == 0 || y == 1 || 1 == b || y == b - 1 || x == 0 || x == r);
		}

		public class Tile : Entity
		{
			public readonly Point MapPosition;
			public Entity occupant { get; private set; }
			private bool isStatic;

			public Tile(Point mapPosition, int tileFrameIdx, bool isStatic)
			{
				this.MapPosition = mapPosition;
				this.isStatic = isStatic;
				this.Position = mapPosition.ToVector2() * new Vector2(16, 16);
				new Sprite(Assets.Texture2D.tiles, frameSize: new Coldsteel.Size(16, 16))
				{
					Origin = new Vector2(8, 8),
					FrameIndex = tileFrameIdx
				}
				.AddToEntity(this);
			}

			public bool IsTraversable => !isStatic && !(occupant is Turret);

			internal void SetOccupant(Entity entity)
			{
				this.occupant = entity;
				entity.Position = this.Position;
			}
		}
	}
}
