#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

#endregion
namespace Lab5
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Game
	{
		// Declarar variavais da classe
		const int WINDOW_WIDTH = 800;
		const int WINDOW_HEIGHT = 600;
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		Texture2D bear0;
		Texture2D bear1;
		Texture2D bear2;
		Rectangle drawRectangle0;
		Rectangle drawRectangle1;
		Rectangle drawRectangle2;

		public Game1 ()
		{
			// Constructor
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";
			graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
			graphics.PreferredBackBufferWidth = WINDOW_HEIGHT;
			graphics.IsFullScreen = false;		
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize ()
		{
			// TODO: Add your initialization logic here
			base.Initialize ();
				
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent ()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch (GraphicsDevice);

			// Load teddybears
			bear0 = Content.Load<Texture2D> ("teddybear0");
			bear1 = Content.Load<Texture2D> ("teddybear1");
			bear2 = Content.Load<Texture2D> ("teddybear2");
			drawRectangle0 = new Rectangle (100, 100, bear0.Width, bear0.Height);
			drawRectangle1 = new Rectangle (200, 100, bear1.Width, bear1.Height);
			drawRectangle2 = new Rectangle (300, 100, bear2.Width, bear2.Height);
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update (GameTime gameTime)
		{
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed) {
				Exit ();
			}
			// TODO: Add your update logic here			
			base.Update (gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.CornflowerBlue);
		
			//TODO: Add your drawing code here
			spriteBatch.Begin ();

			spriteBatch.Draw (bear0, drawRectangle0, Color.White);
			spriteBatch.Draw (bear1, drawRectangle1, Color.White);
			spriteBatch.Draw (bear2, drawRectangle2, Color.White);

			spriteBatch.End ();
            
			base.Draw (gameTime);
		}
	}
}

