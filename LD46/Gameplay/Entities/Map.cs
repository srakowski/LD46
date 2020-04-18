using Coldsteel;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace LD46.Gameplay.Entities
{
	class Map : Entity
	{
		public const int MapDimX = 13;
		public const int MapDimY = 10;

		private Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();

		public Map()
		{
			for (int y = 0; y < MapDimY; y++)
				for (int x = 0; x < MapDimX; x++)
				{
					var tilePosition = new Point(x, y);
					var tile = new Tile(tilePosition);
					tiles[tilePosition] = tile;
					this.AddChild(tile);
				}

			var drop = new StaticElectricity();
			this.AddChild(drop);
			tiles[new Point(8, 8)].PlaceActorOnTile(drop);
		}

		internal Tile GetTargetTile(Point position) => tiles.TryGetValue(position, out var tile) ? tile : null;

		internal Map PlacePlayer(Player player)
		{
			var tile = tiles[new Point(MapDimX / 2, MapDimY / 2)];
			tile.PlaceActorOnTile(player);
			return this;
		}
	}
}
