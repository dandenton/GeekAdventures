using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary
{
	public class MobileSprite : Sprite
	{
		public Int32 Speed { get; set; }
		public Direction CurrentDirection;
		internal const int MOVE_UP = -1;
		internal const int MOVE_DOWN = 1;
		internal const int MOVE_LEFT = -1;
		internal const int MOVE_RIGHT = 1;

		public enum Direction
		{
			Right,
			Left
		}

		public void Move(Int32 x, Int32 y)
		{
			Position.X = x;
			Position.Y = y;
		}

		public MobileSprite(GameContext context, String assetName)
			: base(context, assetName, SpriteType.Moving)
		{
		}

		public MobileSprite(GameContext context, String assetName, Boolean isAnimated)
			: base(context, assetName, SpriteType.Moving, isAnimated)
		{
		}
	}
}
