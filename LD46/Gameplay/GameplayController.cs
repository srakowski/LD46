using Coldsteel;

namespace LD46.Gameplay
{
	class GameplayController : Behavior
	{
		protected override void Start()
		{
			PlaySong(Assets.Song.ninety, loop: true);
		}
	}
}
