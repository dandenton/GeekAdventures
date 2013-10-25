using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace GameLibrary
{
	public class Ink : MobileSprite
	{
		#region Properties

		public enum Animations
		{
			ShootLeft,
			ShootRight
		}

		public const int SPEED = 200;
		private const string _ASSETNAME = "Ink";
		private const int _MOVE_LEFT = -1;
		private const int _MOVE_RIGHT = 1;

		private const int _INK_HEIGHT = 21;
		private const int _INK_WIDTH = 25;
		private const int _INK_FRAMES = 5;

		private Vector2 _Direction = Vector2.Zero;
		private Vector2 _Speed = Vector2.Zero;
		private MobileSprite.Direction _CurrentDirection;
		private Geek _Geek;
		
		#endregion

		public Ink(GameContext context, Geek geek)
			: base(context, _ASSETNAME, true)
		{
			Scale = 1f;
			_Geek = geek;
			float positionY = _Geek.Position.Y + (float)_Geek.Source.Height * _Geek.Scale / 2;
			float positionX = _Geek.Position.X;
			_CurrentDirection = _Geek.CurrentDirection;

			if (_CurrentDirection == MobileSprite.Direction.Right)
			{
				positionX = positionX + _Geek.Source.Width * _Geek.Scale;
			}
			
			Position = new Vector2(positionX, positionY);
			Source = new Rectangle(0, 0, _INK_WIDTH, _INK_HEIGHT);

			AddAnimation(Animations.ShootRight.ToString(), 0, 0, _INK_WIDTH, _INK_HEIGHT, _INK_FRAMES, .1f, 1);
			AddAnimation(Animations.ShootLeft.ToString(), 0, _INK_HEIGHT, _INK_WIDTH, _INK_HEIGHT, _INK_FRAMES, .1f, 1);
		}

		public void Update(GameTime gameTime, int speed)
		{
			if (IsAnimating)
			{
				_Speed.X = speed;
				if (_CurrentDirection == MobileSprite.Direction.Left)
				{
					_Direction.X = _MOVE_LEFT;
				}
				else
				{
					_Direction.X = _MOVE_RIGHT;
				}

				Update(gameTime, _Speed, _Direction);
			}
		}

		public void Shoot(MobileSprite.Direction direction)
		{
			if (direction == MobileSprite.Direction.Left)
			{
				ShootLeft();
			}
			else
			{
				ShootRight();
			}
		}

		public void ShootRight()
		{
			RunAnimation(Animations.ShootRight.ToString());
		}

		public void ShootLeft()
		{
			RunAnimation(Animations.ShootLeft.ToString());
		}
	}
}
