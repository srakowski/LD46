﻿using System;

namespace LD46
{
	class Program
	{
		static void Main(string[] args)
		{
			using (var game = new TowerDefenseGame())
				game.Run();
		}
	}
}
