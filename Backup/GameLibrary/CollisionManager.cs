using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameLibrary
{
	public class CollisionManager
	{
		private List<TerrainSprite> _SolidObjects;
		private List<MobileSprite> _MovingObjects;
		private List<ItemSprite> _ItemObjects;
		public const Int32 INTERSECT_VARIANCE = 20;

		public CollisionManager()
		{
			_SolidObjects = new List<TerrainSprite>();
			_MovingObjects = new List<MobileSprite>();
			_ItemObjects = new List<ItemSprite>();
		}

		public void RegisterSolidObject(TerrainSprite solidSprite)
		{
			_SolidObjects.Add(solidSprite);
		}

		public void RegisterMovingObject(MobileSprite movingSprite)
		{
			_MovingObjects.Add(movingSprite);
		}

		public void RegisterItemObject(ItemSprite itemSprite)
		{
			_ItemObjects.Add(itemSprite);
		}

		public void CheckForCollisions()
		{
			CheckForSolidObjects();
			CheckForMobileObjects();
			CheckForItemObjects();
		}

		private void CheckForSolidObjects()
		{
			foreach (TerrainSprite solidSprite in _SolidObjects)
			{
				foreach (TerrainSprite sprite in _SolidObjects)
				{
					if (sprite.Id != solidSprite.Id)
					{
						if (Intersects(solidSprite.CollisionBox, sprite.CollisionBox))
						{
							solidSprite.HandelCollision(sprite);
							sprite.HandelCollision(solidSprite);
						}
					}
				}

				foreach (MobileSprite sprite in _MovingObjects)
				{
					if (Intersects(solidSprite.CollisionBox, sprite.CollisionBox))
					{
						solidSprite.HandelCollision(sprite);
						sprite.HandelCollision(solidSprite);
					}
				}

				foreach (ItemSprite sprite in _ItemObjects)
				{
					if (Intersects(solidSprite.CollisionBox, sprite.CollisionBox))
					{
						solidSprite.HandelCollision(sprite);
						sprite.HandelCollision(solidSprite);
					}
				}
			}
		}

		private void CheckForMobileObjects()
		{
			foreach (MobileSprite mobileSprite in _MovingObjects)
			{
				foreach (TerrainSprite sprite in _SolidObjects)
				{
					if (Intersects(mobileSprite.CollisionBox, sprite.CollisionBox))
					{
						mobileSprite.HandelCollision(sprite);
						sprite.HandelCollision(mobileSprite);
					}
				}

				foreach (MobileSprite sprite in _MovingObjects)
				{
					if (sprite.Id != mobileSprite.Id)
					{
						if (Intersects(mobileSprite.CollisionBox, sprite.CollisionBox))
						{
							mobileSprite.HandelCollision(sprite);
							sprite.HandelCollision(mobileSprite);
						}
					}
				}

				foreach (ItemSprite sprite in _ItemObjects)
				{
					if (Intersects(mobileSprite.CollisionBox, sprite.CollisionBox))
					{
						mobileSprite.HandelCollision(sprite);
						sprite.HandelCollision(mobileSprite);
					}
				}
			}
		}

		private void CheckForItemObjects()
		{
			foreach (ItemSprite itemSprite in _ItemObjects)
			{
				foreach (TerrainSprite sprite in _SolidObjects)
				{
					if (Intersects(itemSprite.CollisionBox, sprite.CollisionBox))
					{
						itemSprite.HandelCollision(sprite);
						sprite.HandelCollision(itemSprite);
					}
				}

				foreach (MobileSprite sprite in _MovingObjects)
				{
					if (Intersects(itemSprite.CollisionBox, sprite.CollisionBox))
					{
						itemSprite.HandelCollision(sprite);
						sprite.HandelCollision(itemSprite);
					}
				}

				foreach (ItemSprite sprite in _ItemObjects)
				{
					if (sprite.Id != itemSprite.Id)
					{
						if (Intersects(itemSprite.CollisionBox, sprite.CollisionBox))
						{
							itemSprite.HandelCollision(sprite);
							sprite.HandelCollision(itemSprite);
						}
					}
				}
			}
		}

		public bool IntersectsX(Rectangle rectA, Rectangle rectB)
		{
			return (rectA.Right + INTERSECT_VARIANCE > rectB.Left &&
				rectA.Left - INTERSECT_VARIANCE < rectB.Right);
		}

		public bool IntersectsY(Rectangle rectA, Rectangle rectB)
		{
			return (rectA.Bottom + INTERSECT_VARIANCE > rectB.Top &&
				rectA.Top - INTERSECT_VARIANCE < rectB.Bottom);
		}

		protected bool Intersects(Rectangle rectA, Rectangle rectB)
		{
			// Returns True if rectA and rectB contain any overlapping points
			return IntersectsX(rectA, rectB) &&
					IntersectsY(rectA, rectB);
		}

		public List<TerrainSprite> GetIntersectingTerrain(Geek character)
		{
			List<TerrainSprite> result = new List<TerrainSprite>();
			foreach (TerrainSprite solidSprite in _SolidObjects)
			{
				if (solidSprite.Id != character.Id)
				{
					if (Intersects(character.CollisionBox, solidSprite.CollisionBox))
					{
						result.Add(solidSprite);
					}
				}
			}
			return result;
		}
	}
}
