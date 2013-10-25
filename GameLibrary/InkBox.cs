using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameLibrary
{
	public class InkBox : ItemSprite
	{
		internal const String _ASSETNAME = "InkBox";
		internal const Double _ITEM_DURATION = 10;

		public InkBox(GameContext context)
			: base(context, _ASSETNAME, _ITEM_DURATION)
		{
			Scale = .5f;
			_ItemExpiringAnimationDuration = 5;
			Source = new Rectangle(0, 0, 100, 100);
			AddExpireAnimation(new FrameAnimation(0, 0, Source.Width, Source.Height, 2, .75f, 0));

			Position.Y = Context.Background.GroundLevel - Height;
		}

		public override void SetItemPropertiesForCollision(Geek character)
		{
			if (!HasExpired)
			{
				character.Ink_Cartridges += 5;

				// expire the object
				_TimeElapsed = ItemDuration;
			}
		}			
	}
}
