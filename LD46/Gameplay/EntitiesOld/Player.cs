using Coldsteel;
using Coldsteel.Audio;
using Coldsteel.UI;
using Coldsteel.UI.Elements;
using Microsoft.Xna.Framework;
using System.Collections;

namespace LD46.Gameplay.EntitiesOld
{
	class Player : Actor
	{
		private Map map;

		public Player(Map map)
		{
			this.map = map;
			this.map.PlacePlayer(this);
			this.AddSprite(
				Assets.Texture2D.hero,
				s =>
				{
					s.FrameIndex = 0;
				},
				frameSize: new Size(16, 16)
			);
			new PlayerController(this).AddToEntity(this);
			Battery = new Battery();
			PaceMaker = new PaceMaker(this).AddToEntity(this);

			var vm = Text.New;
			var view = View.New.AddElement(
				Div.New.AddElement(vm)
			).AddToEntity(this);
			AddComponent(new RelayBehavior(onUpdate: _ =>
			{
				vm.Value = $"{(int)Battery.Voltage}v {PaceMaker.BeatsPerMinute}bpm";
			}));
		}

		public Battery Battery { get; }

		public PaceMaker PaceMaker { get; }

		public Player Action(Point direction)
		{
			var newPosition = OccupyingTile.MapPosition + direction;
			var tile = map.GetTargetTile(newPosition);
			if (tile == null) return this;

			if (tile.Occupant == null)
			{
				tile.PlaceActorOnTile(this);
			}
			else if (tile.Occupant is Drop)
			{
				if (tile.Occupant is StaticElectricity se)
				{
					Battery.Recharge(se.Voltage);
					se.Dead = true;
					tile.ClearOccupant();
				}
			}

			return this;
		}
	}

	internal class PlayerController : Behavior
	{
		private Player player;
		private Controls controls;

		public PlayerController(Player player)
		{
			this.player = player;
		}

		protected override void Initialize()
		{
			controls = Controls.Map(this);
		}

		protected override void Update()
		{
			if (controls.Up.WasPushed())
			{
				player.Action(new Point(0, -1));
			}
			else if (controls.Down.WasPushed())
			{
				player.Action(new Point(0, 1));
			}
			else if (controls.Left.WasPushed())
			{
				player.Action(new Point(-1, 0));
			}
			else if (controls.Right.WasPushed())
			{
				player.Action(new Point(1, 0));
			}
		}
	}

	internal class Battery
	{
		public float Voltage { get; private set; } = 16.0f;

		public void DrawPower()
		{
			Voltage -= (Voltage / 400f);
		}

		public void Recharge(float volts)
		{
			Voltage += volts;
		}
	}

	internal class PaceMaker : Behavior
	{
		private Player player;
		private AudioEmitter audioEmitter;

		public PaceMaker(Player player)
		{
			this.player = player;
			audioEmitter = new AudioEmitter();
			player.AddComponent(audioEmitter);
		}

		public int BeatsPerMinute
		{
			get
			{
				var calcRate = ((int)player.Battery.Voltage) * 10;
				return calcRate - (calcRate % 40);
			}
		}

		protected override void Initialize()
		{
			StartCoroutine(Beat());
		}

		private IEnumerator Beat()
		{
			while (true)
			{
				yield return Wait.Duration(CalculateTimeToNextBeat());
				audioEmitter.Play(Assets.SoundEffect.beat);
				player.Battery.DrawPower();
			}
		}

		private double CalculateTimeToNextBeat()
		{
			return (60.0 * 1000.0) / BeatsPerMinute;
		}
	}
}
