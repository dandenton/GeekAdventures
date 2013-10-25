using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary
{
	public abstract class ItemSprite : Sprite
	{
		internal const String _EXPIRE_ANIMATION = "Expire Animation";
		internal Double _ItemExpiringAnimationDuration;
		internal Double _TimeElapsed;
		public Double ItemDuration { get; private set; }
		public FrameAnimation ItemExpiringAnimation { get; private set; }

		public Boolean HasExpired
		{
			get
			{
				return _TimeElapsed >= ItemDuration;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ItemSprite"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="assetName">Name of the asset.</param>
		/// <param name="itemDuration">Duration of the item (in seconds) before the item expires.</param>
		public ItemSprite(GameContext context, String assetName, Double itemDuration)
			: base(context, assetName, SpriteType.Item, true)
		{
			ItemDuration = itemDuration;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ItemSprite"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="assetName">Name of the asset.</param>
		/// <param name="itemDuration">Duration of the item (in seconds) before the item expires.</param>
		/// <param name="itemExpiringAnimation">The item's expiring animation.</param>
		public ItemSprite(GameContext context, String assetName, Double itemDuration, FrameAnimation itemExpiringAnimation)
			: this(context, assetName, itemDuration)
		{
			AddExpireAnimation(itemExpiringAnimation);
		}

		public virtual void AddExpireAnimation(FrameAnimation animation)
		{
			ItemExpiringAnimation = animation;

			_Animations.Clear();
			_Animations.Add(_EXPIRE_ANIMATION, ItemExpiringAnimation);
			_ItemExpiringAnimationDuration = (double)ItemExpiringAnimation.FrameLength * ItemExpiringAnimation.FrameCount;
		}

		/// <summary>
		/// Sets whatever properties this item affects on the character.
		/// </summary>
		/// <param name="character">The character.</param>
		public abstract void SetItemPropertiesForCollision(Geek character);

		public override void Update(GameTime gameTime)
		{
			if (!HasExpired)
			{
				_TimeElapsed += gameTime.ElapsedGameTime.TotalSeconds;

				if (!IsAnimating && _TimeElapsed >= _ItemExpiringAnimationDuration)
				{
					RunAnimation(_EXPIRE_ANIMATION);
				}
				else if (IsAnimating)
				{
					base.Update(gameTime);
				}
			}
			else
			{
				IsAnimating = false;
			}
		}

		public override void Draw()
		{
			// if we aren't animating yet and the item still exists then draw it
			// from the original image
			if (!HasExpired && !IsAnimating)
			{
				Context.SpriteBatch.Draw(_SpriteTexture, Position, Source,
					Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
			}
			else if (IsAnimating)
			{
				base.Draw();
			}
		}
	}
}
