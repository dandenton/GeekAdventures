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
	public class Geek : MobileSprite
	{
		#region Properties

		internal const string ASSETNAME = "Geek";
		public const int START_POSITION_X = 325;
		public const int START_POSITION_Y = 245;
		internal const int _MAX_JUMP_HEIGHT = 70;
		internal const int _RELOADING_TIME = 1;

		private const int _WALK_IMAGE_CHANGE_DELAY = 10;
		private int _Walk_Image_Change_Count = 0;
		public int Ink_Ammo = 5;
		public int Ink_Cartridges = 5;

		enum State
		{
			Walking,
			Jumping
		}

		enum ImageState
		{
			Standing,
			Walking,
			Jumping,
		}

		private List<Ink> _InkShots;
		public Boolean IsWalking { get; set; }
		private State _CurrentState = State.Walking;
		private ImageState _CurrentImageState = ImageState.Standing;
		private Vector2 _StartingPosition = Vector2.Zero;

		private ImageState _PreJumpImageState = ImageState.Standing;
		private Rectangle _PreJumpSource;

		private Vector2 _Direction = Vector2.Zero;
		private Vector2 _Speed = Vector2.Zero;
		private Boolean _Reloading
		{
			get
			{
				return _ReloadingCount != 0;;
			}
		}
		private Double _ReloadingCount = 0;

		private KeyboardState _PreviousKeyboardState;

		private Background _Background
		{
			get
			{
				return Context.Background;
			}
		}

		public override Rectangle CollisionBox
		{
			get
			{
				return new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Width, Height);
			}
		}

		public override Int32 Width
		{
			get
			{
				return Convert.ToInt32(_WALKING_RIGHT_WIDTH * Scale);
			}
		}

		public override Int32 Height
		{
			get
			{
				return Convert.ToInt32(_WALKING_RIGHT_HEIGHT * Scale);
			}
		}

		#endregion

		#region Image Position Constants

		private const int _STANDING_RIGHT_X = 0;
		private const int _STANDING_RIGHT_Y = 0;
		private const int _STANDING_RIGHT_WIDTH = 100;
		private const int _STANDING_RIGHT_HEIGHT = 168;

		private const int _WALKING_RIGHT_X = 104;
		private const int _WALKING_RIGHT_Y = 0;
		private const int _WALKING_RIGHT_WIDTH = 118;
		private const int _WALKING_RIGHT_HEIGHT = 168;

		private const int _JUMPING_RIGHT_X = 230;
		private const int _JUMPING_RIGHT_Y = 0;
		private const int _JUMPING_RIGHT_WIDTH = 100;
		private const int _JUMPING_RIGHT_HEIGHT = 168;


		private const int _STANDING_LEFT_X = 572;
		private const int _STANDING_LEFT_Y = 0;
		private const int _STANDING_LEFT_WIDTH = 100;
		private const int _STANDING_LEFT_HEIGHT = 168;

		private const int _WALKING_LEFT_X = 442;
		private const int _WALKING_LEFT_Y = 0;
		private const int _WALKING_LEFT_WIDTH = 118;
		private const int _WALKING_LEFT_HEIGHT = 168;

		private const int _JUMPING_LEFT_X = 338;
		private const int _JUMPING_LEFT_Y = 0;
		private const int _JUMPING_LEFT_WIDTH = 100;
		private const int _JUMPING_LEFT_HEIGHT = 168;

		#endregion

		public Geek(GameContext context)
			: base(context, ASSETNAME)
		{
			Scale = 0.5f;
			Speed = 160;
			_InkShots = new List<Ink>();
			Position = new Vector2(START_POSITION_X, START_POSITION_Y);
			Source = new Rectangle(_STANDING_RIGHT_X, _STANDING_RIGHT_Y, _STANDING_RIGHT_WIDTH, _STANDING_RIGHT_HEIGHT);
		}

		public void Update(GameTime gameTime)
		{
			KeyboardState currentKeyboardState = Keyboard.GetState();

			UpdateMovement(currentKeyboardState);
			UpdateJump(currentKeyboardState);
			UpdateInk(gameTime, currentKeyboardState);

			_PreviousKeyboardState = currentKeyboardState;

			base.Update(gameTime, _Speed, _Direction);
		}

		public override void Draw()
		{
			base.Draw();

			foreach (Ink ink in _InkShots)
			{
				ink.Draw();
			}
		}

		internal virtual void UpdateMovement(KeyboardState currentKeyboardState)
		{
			if (_CurrentState == State.Walking)
			{
				IsWalking = false;
				_Speed = Vector2.Zero;
				_Direction = Vector2.Zero;

				if (currentKeyboardState.IsKeyDown(Keys.Left))
				{
					// if the character can walk without the background moving then
					// move the actual character.  Otherwise the background will move instead
					if (_Background.CanCharacterWalkLeft(this))
					{
						_Speed.X = Speed;
						_Direction.X = MOVE_LEFT;
					}
					IsWalking = true;
					CurrentDirection = Direction.Left;
					if (!_PreviousKeyboardState.IsKeyDown(Keys.Left))
					{
						Source = new Rectangle(_WALKING_LEFT_X, _WALKING_LEFT_Y, _WALKING_LEFT_WIDTH, _WALKING_LEFT_HEIGHT);
						_CurrentImageState = ImageState.Walking;
					}
				}
				else if (currentKeyboardState.IsKeyDown(Keys.Right))
				{
					// if the character can walk without the background moving then
					// move the actual character.  Otherwise the background will move instead
					if (_Background.CanCharacterWalkRight(this))
					{
						_Speed.X = Speed;
						_Direction.X = MOVE_RIGHT;
					}
					IsWalking = true;
					CurrentDirection = Direction.Right;
					if (!_PreviousKeyboardState.IsKeyDown(Keys.Right))
					{
						Source = new Rectangle(_WALKING_RIGHT_X, _WALKING_RIGHT_Y, _WALKING_RIGHT_WIDTH, _WALKING_RIGHT_HEIGHT);
						_CurrentImageState = ImageState.Walking;
					}
				}

				if (IsWalking && _Walk_Image_Change_Count == _WALK_IMAGE_CHANGE_DELAY)
				{
					// log that the character is walking to the background but only
					// if the image is actually moving
					if (_Speed.X > 0)
					{
						_Background.TrackCharacterWalking(Speed, CurrentDirection);
					}

					// toggle the walking images back and forth to look like the character is walking
					switch (_CurrentImageState)
					{
						case ImageState.Jumping:
							if (CurrentDirection == Direction.Right)
							{
								Source = new Rectangle(_WALKING_RIGHT_X, _WALKING_RIGHT_Y, _WALKING_RIGHT_WIDTH, _WALKING_RIGHT_HEIGHT);
							}
							else
							{
								Source = new Rectangle(_WALKING_LEFT_X, _WALKING_LEFT_Y, _WALKING_LEFT_WIDTH, _WALKING_LEFT_HEIGHT);
							}

							_CurrentImageState = ImageState.Walking;
							break;

						case ImageState.Walking:
							if (CurrentDirection == Direction.Right)
							{
								Source = new Rectangle(_JUMPING_RIGHT_X, _JUMPING_RIGHT_Y, _JUMPING_RIGHT_WIDTH, _JUMPING_RIGHT_HEIGHT);
							}
							else
							{
								Source = new Rectangle(_JUMPING_LEFT_X, _JUMPING_LEFT_Y, _JUMPING_LEFT_WIDTH, _JUMPING_LEFT_HEIGHT);
							}

							_CurrentImageState = ImageState.Jumping;
							break;
					}

					_Walk_Image_Change_Count = 0;
				}
				else if (!IsWalking && (_CurrentImageState == ImageState.Walking || _CurrentImageState == ImageState.Jumping))
				{
					// if the character stops we need to switch the image back to the correct standing image
					if (CurrentDirection == Direction.Right)
					{
						Source = new Rectangle(_STANDING_RIGHT_X, _STANDING_RIGHT_Y, _STANDING_RIGHT_WIDTH, _STANDING_RIGHT_HEIGHT);
					}
					else
					{
						Source = new Rectangle(_STANDING_LEFT_X, _STANDING_LEFT_Y, _STANDING_LEFT_WIDTH, _STANDING_LEFT_HEIGHT);
					}
					_Walk_Image_Change_Count = 0;
				}
				else if (IsWalking && _Walk_Image_Change_Count < _WALK_IMAGE_CHANGE_DELAY)
				{
					_Walk_Image_Change_Count++;
				}
			}
		}

		private void UpdateJump(KeyboardState _CurrentKeyboardState)
		{
			if (_CurrentState == State.Walking)
			{
				if (_CurrentKeyboardState.IsKeyDown(Keys.Space) && _PreviousKeyboardState.IsKeyDown(Keys.Space))
				{
					Jump();
				}
			}

			if (_CurrentState == State.Jumping)
			{
				if ((CurrentDirection == Direction.Left && !_Background.CanCharacterWalkLeft(this)) ||
					(CurrentDirection == Direction.Right && !_Background.CanCharacterWalkRight(this)))
				{
					_Speed = new Vector2(0, Speed);
				}

				if (_StartingPosition.Y - Position.Y > _MAX_JUMP_HEIGHT)
				{
					_Direction.Y = MOVE_DOWN;
				}

				if (Position.Y > _StartingPosition.Y)
				{
					Position.Y = _StartingPosition.Y;
					ResetAfterJump();
				}
				else if (_Direction.Y == MOVE_DOWN)
				{
					if (Context.Background.IsCharacterInTheAir(this))
					{
						_Direction.Y = MOVE_DOWN;
					}
					else
					{
						_Direction.Y = 0;
						ResetAfterJump();
					}
				}
			}
			else if (Context.Background.IsCharacterInTheAir(this))
			{
				_Speed = new Vector2(0, Speed);
				_Direction.Y = MOVE_DOWN;
			}
			else
			{
				_Direction.Y = 0;
			}
		}

		private void ResetAfterJump()
		{
			_CurrentState = State.Walking;
			Source = _PreJumpSource;
			_CurrentImageState = _PreJumpImageState;
		}

		private void Jump()
		{
			if (_CurrentState != State.Jumping)
			{
				_PreJumpImageState = _CurrentImageState;
				_PreJumpSource = Source;

				_CurrentState = State.Jumping;
				_StartingPosition = Position;
				_Direction.Y = MOVE_UP;
				_Speed = new Vector2(Speed, Speed);
				_CurrentImageState = ImageState.Jumping;

				if (CurrentDirection == Direction.Right)
				{
					Source = new Rectangle(_JUMPING_RIGHT_X, _JUMPING_RIGHT_Y, _JUMPING_RIGHT_WIDTH, _JUMPING_RIGHT_HEIGHT);
				}
				else
				{
					Source = new Rectangle(_JUMPING_LEFT_X, _JUMPING_LEFT_Y, _JUMPING_LEFT_WIDTH, _JUMPING_LEFT_HEIGHT);
				}
			}
		}

		private void UpdateInk(GameTime gameTime, KeyboardState currentKeyboardState)
		{
			if (Ink_Ammo > 0 && !_Reloading && !_PreviousKeyboardState.IsKeyDown(Keys.LeftControl) && currentKeyboardState.IsKeyDown(Keys.LeftControl))
			{
				Ink ink = new Ink(Context, this);
				_InkShots.Add(ink);
				ink.Shoot(CurrentDirection);
				Ink_Ammo--;
			}

			if (Ink_Cartridges > 0 && _Reloading || currentKeyboardState.IsKeyDown(Keys.LeftAlt))
			{
				_ReloadingCount += gameTime.ElapsedGameTime.TotalSeconds;
			}

			if (_Reloading && _ReloadingCount >= _RELOADING_TIME)
			{
				Ink_Cartridges--;
				Ink_Ammo = 5;
				_ReloadingCount = 0;
			}

			// remove any spent ink
			for (int i = _InkShots.Count - 1; i > -1; i--)
            {
				if (!_InkShots[i].IsAnimating)
				{
					_InkShots[i] = null;
					_InkShots.RemoveAt(i);
				}
			}

			foreach (Ink ink in _InkShots)
			{
				// update the ink with the default speed plus an motion of the character
				ink.Update(gameTime, Ink.SPEED + Convert.ToInt32(_Speed.X));
			}
		}

		public override void HandelCollision(TerrainSprite solidObject)
		{
		}

		public override void HandelCollision(MobileSprite movingObject)
		{
		}

		public override void HandelCollision(ItemSprite itemObject)
		{
			itemObject.SetItemPropertiesForCollision(this);
		}
	}
}
