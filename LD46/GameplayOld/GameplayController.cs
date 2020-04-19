using Coldsteel;

namespace LD46.GameplayOld
{
	class GameplayController : Behavior
	{
		protected override void Start()
		{
			PlaySong(Assets.Song.ninety, loop: true);
		}
	}
}
