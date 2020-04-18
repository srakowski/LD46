using Coldsteel;

namespace LD46.Gameplay.Entities
{
	abstract class Actor : Entity
	{
		public Tile OccupyingTile { get; set; }
	}
}
