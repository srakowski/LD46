using Coldsteel;
using Microsoft.Xna.Framework;
using System;

namespace LD46.Gameplay.Entities
{
	class Tile : Entity
	{
		public static int TileDim = 72;

		public Tile(Point mapPosition)
		{
			MapPosition = mapPosition;
			Position = mapPosition.ToVector2() * TileDim;
			this.AddSprite(Assets.Texture2D.tile);
			this.IsTraversable = true;
		}

		public Point MapPosition { get; }

		public bool IsTraversable { get; }

		public Actor Occupant { get; private set; }

		public Tile PlaceActorOnTile(Actor actor)
		{
			if (actor.OccupyingTile != null)
				actor.OccupyingTile.ClearOccupant();

			actor.Position = this.Position;
			this.Occupant = actor;
			actor.OccupyingTile = this;
			return this;
		}

		internal void ClearOccupant()
		{
			this.Occupant = null;
		}
	}
}
