using System;
using System.Collections.Generic;
using System.Text;

namespace LD46.Content
{
	public class TileAtlas
	{
		public const int TopLeftEdge = 0;
		public const int TopRightEdge = 5;
		public static readonly int[] TopRow = new[] { 1, 2, 3, 4 };
		public static readonly int[] TopWall = new[] { 7, 8, 9, 10 };
		public static readonly int[] LeftCol = new[] { 6, 12, 18, 24 };
		public static readonly int[] RightCol = new[] { 11, 17, 23, 29 };
		public static readonly int[] BotRow = new[] { 31, 32, 33, 34 };
		public const int BottomLeftEdge = 30;
		public const int BottomRightEdge = 35;

		public static readonly int[] Floor = new[] { 13, 16, 19, 22, 25, 28 };
	}
}
