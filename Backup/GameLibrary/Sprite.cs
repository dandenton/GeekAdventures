using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace GameLibrary
{
	public class Sprite
	{
		#region Properties

		public Guid Id { get; private set; }
		public Vector2 Position;
		internal Texture2D _SpriteTexture;

		/// <summary>
		/// Gets or sets the asset name for the Sprite's Texture
		/// </summary>
		public string AssetName;

		/// <summary>
		/// The Size of the Sprite (with scale applied)
		/// </summary>
		public Rectangle Size;

		private float _Scale = 1.0f;
		/// <summary>
		/// The amount to increase/decrease the size of the original sprite. 
		/// </summary>
		public float Scale
		{
			get { return _Scale; }
			set
			{
				_Scale = value;
				//Recalculate the Size of the Sprite with the new scale
				Size = new Rectangle(0, 0, (int)(_SpriteTexture.Width * Scale), (int)(_SpriteTexture.Height * Scale));
			}
		}

		Rectangle source;
		public Rectangle Source
		{
			get { return source; }
			set
			{
				source = value;
				Size = new Rectangle(0, 0, (int)(source.Width * Scale), (int)(source.Height * Scale));
			}
		}

		public virtual Rectangle CollisionBox
		{
			get
			{
				return new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Width, Height);
			}
		}

		public virtual Int32 Width
		{
			get
			{
				return Size.Width;
			}
		}

		public virtual Int32 Height
		{
			get
			{
				return Size.Height;
			}
		}

		public SpriteType Type { get; set; }

		public enum SpriteType
		{
			NonSolid,
			Solid,
			Moving,
			Item
		}

		public GameContext Context { get; private set; }


		#region Animation Properties

		public Boolean IsAnimated { get; set; }
		public Boolean IsAnimating { get; set; }
		internal Dictionary<String, FrameAnimation> _Animations;

		/// <summary>
		/// The FrameAnimation object of the currently playing animation
		/// </summary>
		public FrameAnimation CurrentFrameAnimation
		{
			get
			{
				if (!string.IsNullOrEmpty(CurrentAnimation))
				{
					return _Animations[CurrentAnimation];
				}

				return null;
			}
		}

		private string _CurrentAnimation;
		/// <summary>
		/// The string name of the currently playing animaton.  Setting the animation
		/// resets the CurrentFrame and PlayCount properties to zero.
		/// </summary>
		public string CurrentAnimation
		{
			get { return _CurrentAnimation; }
			set
			{
				if (_Animations.ContainsKey(value))
				{
					_CurrentAnimation = value;
					_Animations[_CurrentAnimation].CurrentFrame = 0;
					_Animations[_CurrentAnimation].PlayCount = 0;
				}
			}
		}

		#endregion

		#endregion

		public Sprite(GameContext context, String assetName, SpriteType type)
		{
			Id = Guid.NewGuid();
			Context = context;
			Type = type;
			AssetName = assetName;
			_Animations = new Dictionary<String, FrameAnimation>();
			_SpriteTexture = Context.ContentManager.Load<Texture2D>(AssetName);
			Position = new Vector2(0, 0);
			Source = new Rectangle(0, 0, _SpriteTexture.Width, _SpriteTexture.Height);

			switch (type)
			{
				case SpriteType.Solid:
					if (typeof(TerrainSprite).IsAssignableFrom(this.GetType()))
					{
						context.CollisionManager.RegisterSolidObject((TerrainSprite)this);
					}
					else
					{
						throw new InvalidCastException("You cannot have a sprite type of Moving that does not inherit from TerrainSprite.");
					}
					break;

				case SpriteType.Moving:
					if (typeof(MobileSprite).IsAssignableFrom(this.GetType()))
					{
						context.CollisionManager.RegisterMovingObject((MobileSprite)this);
					}
					else
					{
						throw new InvalidCastException("You cannot have a sprite type of Moving that does not inherit from MobileSprite.");
					}
					break;

				case SpriteType.Item:
					if (typeof(ItemSprite).IsAssignableFrom(this.GetType()))
					{
						context.CollisionManager.RegisterItemObject((ItemSprite)this);
					}
					else
					{
						throw new InvalidCastException("You cannot have a sprite type of Item that does not inherit from ItemSprite.");
					}
					break;
			}
		}

		public Sprite(GameContext context, String assetName, SpriteType type, Boolean isAnimated)
			: this(context, assetName, type)
		{
			IsAnimated = isAnimated;
		}

		public virtual void Draw()
		{
			if (!IsAnimated)
			{
				Context.SpriteBatch.Draw(_SpriteTexture, Position, Source,
					Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
			}
			else if (IsAnimated && IsAnimating)
			{
				Context.SpriteBatch.Draw(_SpriteTexture, Position, CurrentFrameAnimation.FrameRectangle,
					Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
			}
		}

		public virtual void Update(GameTime gameTime)
		{
			// Don't do anything if the sprite is not animating
			if (IsAnimating)
			{
				UpdateAnimation(gameTime);
			}
		}

		public virtual void Update(GameTime gameTime, Vector2 speed, Vector2 direction)
		{
			Position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

			// Don't do anything if the sprite is not animating
			if (IsAnimating)
			{
				UpdateAnimation(gameTime);
			}
		}

		public virtual void UpdateAnimation(GameTime gameTime)
		{
			// If there is not a currently active animation
			if (CurrentFrameAnimation == null)
			{
				// Make sure we have an animation associated with this sprite
				if (_Animations.Count > 0)
				{
					// Set the active animation to the first animation
					// associated with this sprite
					string[] sKeys = new string[_Animations.Count];
					_Animations.Keys.CopyTo(sKeys, 0);
					CurrentAnimation = sKeys[0];
				}
				else
				{
					return;
				}
			}

			// Run the Animation's update method
			IsAnimating = CurrentFrameAnimation.Update(gameTime);

			// Check to see if there is a "followup" animation named for this animation
			if (!String.IsNullOrEmpty(CurrentFrameAnimation.NextAnimation))
			{
				// If there is, see if the currently playing animation has
				// completed a full animation loop
				if (CurrentFrameAnimation.PlayCount > 0)
				{
					// If it has, set up the next animation
					CurrentAnimation = CurrentFrameAnimation.NextAnimation;
				}
			}
		}

		public virtual void HandelCollision(TerrainSprite solidObject)
		{
		}

		public virtual void HandelCollision(MobileSprite movingObject)
		{
		}

		public virtual void HandelCollision(ItemSprite itemObject)
		{
		}

		public override string ToString()
		{
			return String.Format("Left: {0} Top: {1} Right: {2} Bottom: {3}", CollisionBox.Left, CollisionBox.Top, CollisionBox.Right, CollisionBox.Bottom);
		}

		#region Animation Methods

		public virtual void AddAnimation(string name, int x, int y, int width, int height, int frames, float frameLength, int maxPlayCount)
		{
			_Animations.Add(name, new FrameAnimation(x, y, width, height, frames, frameLength, maxPlayCount));
		}

		public virtual void AddAnimation(string name, FrameAnimation animation)
		{
			_Animations.Add(name, animation);
		}

		public virtual void RunAnimation(string name)
		{
			CurrentAnimation = name;
			IsAnimating = true;
		}

		public FrameAnimation GetAnimationByName(string name)
		{
			if (_Animations.ContainsKey(name))
			{
				return _Animations[name];
			}
			else
			{
				return null;
			}
		}
		
		#endregion
	}
}
