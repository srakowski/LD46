using Coldsteel;

namespace LD46.Shaders
{
	class Fade : Shader
	{
		public float Percent { get; set; } = 1.0f;

		public Fade() : base(Assets.Effect.fade)
		{
		}

		public override void ApplyParameters()
		{
			if (Effect == null) return;
			Effect.Parameters[nameof(Percent)].SetValue(Percent);
		}
	}
}
