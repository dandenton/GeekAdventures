using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameLibrary
{
	public class FrameAnimation
	{
		#region Properties

		/// <summary>
		/// The first frame of the Animation.  We will calculate other frames on the fly based on this frame.
		/// </summary>
		private Rectangle _InitialFrame;

		/// <summary>
		/// The number of frames the animation contains
		/// </summary>
		public int FrameCount { get; set; }

		/// <summary>
		/// The time (in seconds) to display each frame
		/// </summary>
		public float FrameLength { get; set; }

		public float FrameTimer { get; set; }

		private int _CurrentFrame;
		/// <summary>
		/// The frame number currently being displayed
		/// </summary>
		public int CurrentFrame
		{
			get { return _CurrentFrame; }
			set { _CurrentFrame = (int)MathHelper.Clamp(value, 0, FrameCount - 1); }
		}

		public int FrameWidth
		{
			get { return _InitialFrame.Width; }
		}

		public int FrameHeight
		{
			get { return _InitialFrame.Height; }
		}

		/// <summary>
		/// The rectangle associated with the current
		/// animation frame.
		/// </summary>
		public Rectangle FrameRectangle
		{
			get
			{
				return new Rectangle(
					_InitialFrame.X + (_InitialFrame.Width * CurrentFrame),
					_InitialFrame.Y, _InitialFrame.Width, _InitialFrame.Height);
			}
		}

		public int PlayCount { get; set; }

		public string NextAnimation { get; set; }

		public int MaxPlayCount { get; set; }
		
		#endregion

		public FrameAnimation(Rectangle firstFrame, int frames)
		{
			_InitialFrame = firstFrame;
			FrameCount = frames;
		}

		public FrameAnimation(int x, int y, int width, int height, int frames)
			: this(new Rectangle(x, y, width, height), frames)
		{
			FrameCount = frames;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FrameAnimation"/> class.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="frames">The frames.</param>
		/// <param name="frameLength">Length of the frame.</param>
		/// <param name="maxPlayCount">The max play count.  Pass in 0 for infinite</param>
		public FrameAnimation(int x, int y, int width, int height, int frames, float frameLength, int maxPlayCount)
			: this(new Rectangle(x, y, width, height), frames)
		{
			MaxPlayCount = maxPlayCount;
			FrameLength = frameLength;
		}

		public FrameAnimation(int x, int y, int width, int height, int frames, float frameLength, string nextAnimation)
			: this(new Rectangle(x, y, width, height), frames)
		{
			FrameLength = frameLength;
			NextAnimation = nextAnimation;
		}

		/// <summary>
		/// Updates the specified game time.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <returns>Returns true if the animation is continuting</returns>
		public bool Update(GameTime gameTime)
		{
			FrameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

			if (FrameTimer > FrameLength)
			{
				FrameTimer = 0.0f;
				CurrentFrame = (CurrentFrame + 1) % FrameCount;
				if (CurrentFrame == 0)
				{
					PlayCount = (int)MathHelper.Min(PlayCount + 1, int.MaxValue);
					if (PlayCount == MaxPlayCount)
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
