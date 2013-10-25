using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameLibrary
{
	//public class AnimatedSprite : Sprite
	//{
	//    #region Properties

	//    public Boolean IsAnimating { get; set; }
	//    internal Dictionary<String, FrameAnimation> _Animations;

	//    /// <summary>
	//    /// The FrameAnimation object of the currently playing animation
	//    /// </summary>
	//    public FrameAnimation CurrentFrameAnimation
	//    {
	//        get
	//        {
	//            if (!string.IsNullOrEmpty(CurrentAnimation))
	//            {
	//                return _Animations[CurrentAnimation];
	//            }

	//            return null;
	//        }
	//    }

	//    private string _CurrentAnimation;
	//    /// <summary>
	//    /// The string name of the currently playing animaton.  Setting the animation
	//    /// resets the CurrentFrame and PlayCount properties to zero.
	//    /// </summary>
	//    public string CurrentAnimation
	//    {
	//        get { return _CurrentAnimation; }
	//        set
	//        {
	//            if (_Animations.ContainsKey(value))
	//            {
	//                _CurrentAnimation = value;
	//                _Animations[_CurrentAnimation].CurrentFrame = 0;
	//                _Animations[_CurrentAnimation].PlayCount = 0;
	//            }
	//        }
	//    }
		
	//    #endregion

	//    public AnimatedSprite(GameContext context, String assetName, SpriteType type)
	//        : base(context, assetName, type)
	//    {
	//        _Animations = new Dictionary<String, FrameAnimation>();
	//    }

	//    public virtual void AddAnimation(string name, int x, int y, int width, int height, int frames, float frameLength, int maxPlayCount)
	//    {
	//        _Animations.Add(name, new FrameAnimation(x, y, width, height, frames, frameLength, maxPlayCount));
	//    }

	//    public virtual void AddAnimation(string name, FrameAnimation animation)
	//    {
	//        _Animations.Add(name, animation);
	//    }

	//    public virtual void RunAnimation(string name)
	//    {
	//        CurrentAnimation = name;
	//        IsAnimating = true;
	//    }

	//    public virtual void Update(GameTime gameTime)
	//    {
	//        // Don't do anything if the sprite is not animating
	//        if (IsAnimating)
	//        {
	//            // If there is not a currently active animation
	//            if (CurrentFrameAnimation == null)
	//            {
	//                // Make sure we have an animation associated with this sprite
	//                if (_Animations.Count > 0)
	//                {
	//                    // Set the active animation to the first animation
	//                    // associated with this sprite
	//                    string[] sKeys = new string[_Animations.Count];
	//                    _Animations.Keys.CopyTo(sKeys, 0);
	//                    CurrentAnimation = sKeys[0];
	//                }
	//                else
	//                {
	//                    return;
	//                }
	//            }

	//            // Run the Animation's update method
	//            IsAnimating = CurrentFrameAnimation.Update(gameTime);

	//            // Check to see if there is a "followup" animation named for this animation
	//            if (!String.IsNullOrEmpty(CurrentFrameAnimation.NextAnimation))
	//            {
	//                // If there is, see if the currently playing animation has
	//                // completed a full animation loop
	//                if (CurrentFrameAnimation.PlayCount > 0)
	//                {
	//                    // If it has, set up the next animation
	//                    CurrentAnimation = CurrentFrameAnimation.NextAnimation;
	//                }
	//            }
	//        }
	//    }

	//    public override void Update(GameTime gameTime, Vector2 speed, Vector2 direction)
	//    {
	//        base.Update(gameTime, speed, direction);

	//        Update(gameTime);
	//    }

	//    public override void Draw()
	//    {
	//        if (IsAnimating)
	//        {
	//            Context.SpriteBatch.Draw(_SpriteTexture, Position, CurrentFrameAnimation.FrameRectangle,
	//                Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
	//        }
	//    }

	//    public FrameAnimation GetAnimationByName(string name)
	//    {
	//        if (_Animations.ContainsKey(name))
	//        {
	//            return _Animations[name];
	//        }
	//        else
	//        {
	//            return null;
	//        }
	//    }
	//}
}
