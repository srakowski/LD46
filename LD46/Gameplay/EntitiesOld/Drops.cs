using Coldsteel;

namespace LD46.Gameplay.EntitiesOld
{
	abstract class Drop : Actor
	{
		public Drop() { }
	}

	class StaticElectricity : Drop
	{
		public StaticElectricity()
		{
			this.AddSprite(Assets.Texture2D.staticElectricity);
		}

		public float Voltage { get; } = 3.0f;
	}
}
