using Coldsteel;
using LD46.Content;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LD46.GameplayOld
{
	class Map : Entity
	{
		private Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();

		public Map()
		{
			int h = 11; int b = h - 1;
			int w = 100; int r = w - 1;

			var rand = new Random();

			for (int y = 0; y < h; y++)
				for (int x = 0; x < w; x++)
				{
					var pos = new Point(x, y);
					var tileFrame = ComputeTileFrame(b, r, rand, y, x);
					var tile = new Tile(pos, this, tileFrame, y == b);
					this.AddChild(tile);
				}
		}

		private static int ComputeTileFrame(int b, int r, Random rand, int y, int x)
		{
			var tilesFrame = 0;
			if (y == 0 && x == 0) tilesFrame = TileAtlas.TopLeftEdge;
			else if (y == 0 && x == r) tilesFrame = TileAtlas.TopRightEdge;
			else if (y == b && x == 0) tilesFrame = TileAtlas.BottomLeftEdge;
			else if (y == b && x == r) tilesFrame = TileAtlas.BottomRightEdge;
			else if (y == 0) tilesFrame = TileAtlas.TopRow.OrderBy(_ => rand.Next()).First();
			else if (y == 1 && x != 0 && x != r) tilesFrame = TileAtlas.TopWall.OrderBy(_ => rand.Next()).First();
			else if (y == b) tilesFrame = TileAtlas.BotRow.OrderBy(_ => rand.Next()).First();
			else if (x == 0) tilesFrame = TileAtlas.LeftCol.OrderBy(_ => rand.Next()).First();
			else if (x == r) tilesFrame = TileAtlas.RightCol.OrderBy(_ => rand.Next()).First();
			else tilesFrame = TileAtlas.Floor.OrderBy(_ => rand.Next()).First();
			return tilesFrame;
		}
	}

	class Tile : Entity
	{
		private Point mapPosition;
		private Map map;

		public Tile(Point mapPosition, Map map, int tileFrameIdx, bool abovePlayer)
		{
			this.mapPosition = mapPosition;
			this.Position = mapPosition.ToVector2() * new Vector2(16, 16);
			this.map = map;

			new Sprite(Assets.Texture2D.tiles, abovePlayer ? Factory.MapAbovePlayer : Factory.MapBelowPlayer, new Size(16, 16))
			{
				FrameIndex = tileFrameIdx
			}
			.AddToEntity(this);
		}
	}
}
