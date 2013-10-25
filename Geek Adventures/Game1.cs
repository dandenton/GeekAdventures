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
using GameLibrary;

namespace WindowsGame1
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		public GameContext Context;
		private GraphicsDeviceManager _GraphicsManager;
		private SpriteBatch _SpriteBatch;
		private Geek _Geek;
		private SpriteFont _GameFont;

		public Game1()
		{
			_GraphicsManager = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here

			base.Initialize();

			this.IsMouseVisible = true;
			_GameFont = Content.Load<SpriteFont>("Fonts\\Linds");
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			_SpriteBatch = new SpriteBatch(GraphicsDevice);

			Context = new GameContext(Content, _SpriteBatch, _GraphicsManager);

			Context.Background = new Background(Context);
			_Geek = new Geek(Context);

			TerrainSprite block = Context.Background.AddTerrainSprite(TerrainSprite.BlockType.Block01);
			block.Position.X = 1460;
			block.Position.Y = Context.Background.GroundLevel - block.Height;

			block = Context.Background.AddTerrainSprite(TerrainSprite.BlockType.Block01);
			block.Position.X = 940;
			block.Position.Y = Context.Background.GroundLevel - (block.Height * 3);

			block = Context.Background.AddTerrainSprite(TerrainSprite.BlockType.Block01);
			block.Position.X = 720;
			block.Position.Y = Context.Background.GroundLevel - (block.Height * 5);

			block = Context.Background.AddTerrainSprite(TerrainSprite.BlockType.Block01);
			block.Position.X = 1060;
			block.Position.Y = Context.Background.GroundLevel - block.Height;

			block = Context.Background.AddTerrainSprite(TerrainSprite.BlockType.Block01);
			block.Position.X = 1010;
			block.Position.Y = Context.Background.GroundLevel - block.Height;

			block = Context.Background.AddTerrainSprite(TerrainSprite.BlockType.Block01);
			block.Position.X = 1060;
			block.Position.Y = Context.Background.GroundLevel - (block.Height * 2);

			block = Context.Background.AddTerrainSprite(TerrainSprite.BlockType.Block01);
			block.Position.X = 1110;
			block.Position.Y = Context.Background.GroundLevel - (block.Height * 2);

			block = Context.Background.AddTerrainSprite(TerrainSprite.BlockType.Block01);
			block.Position.X = 1110;
			block.Position.Y = Context.Background.GroundLevel - (block.Height * 3);

			block = Context.Background.AddTerrainSprite(TerrainSprite.BlockType.Block01);
			block.Position.X = 1110;
			block.Position.Y = Context.Background.GroundLevel - block.Height;

			block = Context.Background.AddTerrainSprite(TerrainSprite.BlockType.Block01);
			block.Position.X = 1160;
			block.Position.Y = Context.Background.GroundLevel - block.Height;

			block = Context.Background.AddTerrainSprite(TerrainSprite.BlockType.Block01);
			block.Position.X = 1160;
			block.Position.Y = Context.Background.GroundLevel - (block.Height * 2);

			block = Context.Background.AddTerrainSprite(TerrainSprite.BlockType.Block01);
			block.Position.X = 1210;
			block.Position.Y = Context.Background.GroundLevel - block.Height;

			Context.Background.AddBackgroundSprite("Background\\Background01");
			Context.Background.AddBackgroundSprite("Background\\Background02");
			Context.Background.AddBackgroundSprite("Background\\Background03");
			Context.Background.AddBackgroundSprite("Background\\Background04");
			Context.Background.AddBackgroundSprite("Background\\Background05");
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
			{
				this.Exit();
			}

			_Geek.Update(gameTime);
			Context.Background.Update(gameTime, _Geek);
			Context.CollisionManager.CheckForCollisions();

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			_SpriteBatch.Begin();
			
			Context.Background.Draw();
			_Geek.Draw();
			_SpriteBatch.DrawString(_GameFont, String.Format("Health: {0}%   Ink Cartridges: {1}    Ink Shots: {2}", 100, _Geek.Ink_Cartridges, _Geek.Ink_Ammo), Vector2.Zero, Color.Black);

			_SpriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
