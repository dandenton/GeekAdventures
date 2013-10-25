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
	public class GameContext
	{
		public ContentManager ContentManager { get; private set; }
		public SpriteBatch SpriteBatch { get; private set; }
		public CollisionManager CollisionManager { get; private set; }
		public GraphicsDeviceManager GraphicsManager { get; private set; }

		public Background Background { get; set; }

		public GameContext(ContentManager manager, SpriteBatch spriteBatch, GraphicsDeviceManager graphicsManager)
		{
			ContentManager = manager;
			SpriteBatch = spriteBatch;
			GraphicsManager = graphicsManager;
			CollisionManager = new CollisionManager();
		}
	}
}
