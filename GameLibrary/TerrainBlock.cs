using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary
{
	public class TerrainSprite : Sprite
	{
		private const String _TERRAIN_IMAGE_PATH = "Terrain\\{0}";

		public enum BlockType
		{
			Block01
		}

		public TerrainSprite(GameContext context, BlockType type)
			: base(context, String.Format(_TERRAIN_IMAGE_PATH, type.ToString()), SpriteType.Solid)
		{
			Scale = .5f;
		}
	}
}
