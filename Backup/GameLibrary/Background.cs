using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameLibrary
{
	public class Background
	{
		public GameContext Context;
		private List<Sprite> _BackgroundSprites;
		private List<TerrainSprite> _TerrainSprites;
		private float _DistanceTraveled = Geek.START_POSITION_X;
		private const Int32 _INTERSECT_VARIANCE = 20;

		public Int32 MinDistanceToEdge = 150;

		public Int32 GroundLevel = 329;

		public Background(GameContext context)
		{
			_BackgroundSprites = new List<Sprite>();
			_TerrainSprites = new List<TerrainSprite>();
			Context = context;
		}

		public void AddBackgroundSprite(String assetName)
		{
			Sprite backgroundSprite = new Sprite(Context, assetName, Sprite.SpriteType.NonSolid);
			if (_BackgroundSprites.Count > 0)
			{
				backgroundSprite.Position.Y = 50;
				backgroundSprite.Position.X = _BackgroundSprites[_BackgroundSprites.Count - 1].Position.X + _BackgroundSprites[_BackgroundSprites.Count - 1].Size.Width;
			}
			else
			{
				backgroundSprite.Position.Y = 50;
			}
			_BackgroundSprites.Add(backgroundSprite);
			//System.Diagnostics.Debug.WriteLine(String.Format("B{0}: {1}", _BackgroundSprites.Count - 1, backgroundSprite.ToString()));
		}

		public TerrainSprite AddTerrainSprite(TerrainSprite.BlockType type)
		{
			TerrainSprite terrain = new TerrainSprite(Context, type);
			_TerrainSprites.Add(terrain);
			return terrain;
		}

		public void Draw()
		{
			for (int i = 0; i < _BackgroundSprites.Count; i++)
			{
				_BackgroundSprites[i].Draw();
			}

			foreach (TerrainSprite terrainSprite in _TerrainSprites)
			{
				terrainSprite.Draw();
			}
		}

		/// <summary>
		/// Determines whether the character can walk within the border of the background.
		/// </summary>
		/// <param name="characterLocation">The character location.</param>
		/// <param name="characterRectangle">The character rectangle.</param>
		/// <returns>
		/// 	<c>true</c> if this instance [can character walk] the specified character location; otherwise, <c>false</c>.
		/// </returns>
		public Boolean CanCharacterWalkLeft(Geek character)
		{
			List<TerrainSprite> terrain = Context.CollisionManager.GetIntersectingTerrain(character);

			if (!terrain.Exists(t =>
				{
					// make sure the character is actuall on the right side of the terrain
					if (character.CollisionBox.Right - _INTERSECT_VARIANCE > t.CollisionBox.Left)
					{
						return t.Position.X + t.Width >= character.Position.X &&
							(character.CollisionBox.Bottom - _INTERSECT_VARIANCE > t.CollisionBox.Top &&
							character.CollisionBox.Top + _INTERSECT_VARIANCE < t.CollisionBox.Bottom);
					}
					return false;
				}))
			{
				if ((_DistanceTraveled > 0) && (character.Position.X >= MinDistanceToEdge))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Determines whether the character can walk within the border of the background.
		/// </summary>
		/// <param name="characterLocation">The character location.</param>
		/// <param name="characterRectangle">The character rectangle.</param>
		/// <returns>
		/// 	<c>true</c> if this instance [can character walk] the specified character location; otherwise, <c>false</c>.
		/// </returns>
		public Boolean CanCharacterWalkRight(Geek character)
		{
			List<TerrainSprite> terrain = Context.CollisionManager.GetIntersectingTerrain(character);

			float characterRightSide = character.Position.X + character.Width;

			if (!terrain.Exists(t => 
				{
					// make sure the character is actuall on the left side of the terrain
					if (character.Position.X + _INTERSECT_VARIANCE < t.CollisionBox.Right)
					{
						// and if it is that the character isn't going to run into the terrain
						return t.Position.X <= characterRightSide &&
							(character.CollisionBox.Bottom - _INTERSECT_VARIANCE > t.CollisionBox.Top &&
							character.CollisionBox.Top + _INTERSECT_VARIANCE < t.CollisionBox.Bottom);
					}
					return false;
				}))
			{
				if (characterRightSide <= Context.GraphicsManager.GraphicsDevice.Viewport.Width - MinDistanceToEdge)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Determines whether the character is in the air and not on any type of terrain.
		/// </summary>
		/// <param name="character">The character.</param>
		/// <returns>
		/// 	<c>true</c> if [is character in the air] [the specified character]; otherwise, <c>false</c>.
		/// </returns>
		public Boolean IsCharacterInTheAir(Geek character)
		{
			List<TerrainSprite> terrain = Context.CollisionManager.GetIntersectingTerrain(character);

			if (!terrain.Exists(t => t.Position.Y < character.CollisionBox.Bottom))
			{
				if (character.CollisionBox.Bottom <= GroundLevel)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Tracks the character walking.
		/// </summary>
		/// <param name="speed">The speed.</param>
		/// <param name="direction">The direction.</param>
		public void TrackCharacterWalking(Int32 speed, Geek.Direction direction)
		{
			if (direction == Geek.Direction.Left)
			{
				_DistanceTraveled = _DistanceTraveled - speed;
			}
			else
			{
				_DistanceTraveled = _DistanceTraveled + speed;
			}
		}

		public void Update(GameTime gameTime, Geek character)
		{
			Vector2 direction;
			// setup the background speed to match the character's speed
			Vector2 speed = new Vector2(character.Speed, 0);

			if (character.IsWalking && character.Position.X + character.Source.Width > Context.GraphicsManager.GraphicsDevice.Viewport.Width - MinDistanceToEdge)
			{
				direction = new Vector2(-1, 0);

				// move the background to the right
				for (int i = 0; i < _BackgroundSprites.Count; i++)
				{
					// check if we need to move a background panel to the front of the line
					if (_BackgroundSprites[i].Position.X < -_BackgroundSprites[i].Size.Width)
					{
						Sprite lastSpriteInLine;
						if (i > 0)
						{
							lastSpriteInLine = _BackgroundSprites[i - 1];
						}
						else
						{
							lastSpriteInLine = _BackgroundSprites[_BackgroundSprites.Count - 1];
						}
						_BackgroundSprites[i].Position.X = lastSpriteInLine.Position.X + lastSpriteInLine.Size.Width;
					}
					TrackCharacterWalking(Convert.ToInt32(speed.X), Geek.Direction.Right);
					_BackgroundSprites[i].Update(gameTime, speed, direction);
				}

				foreach (TerrainSprite terrainSprite in _TerrainSprites)
				{
					terrainSprite.Update(gameTime, speed, direction);	
				}
			}
			else if (character.IsWalking && _DistanceTraveled > 0 && character.Position.X < MinDistanceToEdge)
			{
				direction = new Vector2(1, 0);

				// move the background to the left
				for (int i = _BackgroundSprites.Count - 1; i > -1; i--)
				{
					// check if we need to move a background panel to the front of the line
					if (_BackgroundSprites[i].Position.X > Context.GraphicsManager.GraphicsDevice.Viewport.Width)
					{
						Sprite lastSpriteInLine;
						if (i < _BackgroundSprites.Count - 1)
						{
							lastSpriteInLine = _BackgroundSprites[i + 1];
						}
						else
						{
							lastSpriteInLine = _BackgroundSprites[0];
						}
						_BackgroundSprites[i].Position.X = lastSpriteInLine.Position.X - _BackgroundSprites[i].Size.Width;
					}
					TrackCharacterWalking(Convert.ToInt32(speed.X), Geek.Direction.Left);
					_BackgroundSprites[i].Update(gameTime, speed, direction);
					//System.Diagnostics.Debug.WriteLine(String.Format("B{0}: {1}", i, _BackgroundSprites[i].ToString()));
				}

				foreach (TerrainSprite terrainSprite in _TerrainSprites)
				{
					terrainSprite.Update(gameTime, speed, direction);
				}
			}
		}
	}
}
