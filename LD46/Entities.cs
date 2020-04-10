using Coldsteel;

namespace LD46
{
	abstract class BaseEntity : Entity
	{
		protected BaseEntity(LDJamGameState state)
		{
			State = state;
		}

		public LDJamGameState State { get; }
	}
}
