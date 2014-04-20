#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

#endregion
namespace Lab6
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Game
	{
		const int WINDOW_WIDTH = 800;
		const int WINDOW_HEIGHT = 600;
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		TeddyBear bear0;
		TeddyBear bear1;


		public Game1 ()
		{
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";	            
			graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
			graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
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

			//TODO: use this.Content to load your game content here
			bear0 = new TeddyBear (Content, "teddybear0", 100, 100, WINDOW_WIDTH, 
				WINDOW_HEIGHT);
			bear1 = new TeddyBear (Content, "teddybear1", 200, 100, WINDOW_WIDTH, 
				WINDOW_HEIGHT);
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
			bear0.Update ();
			bear1.Update ();
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

			bear0.Draw (spriteBatch);
			bear1.Draw (spriteBatch);

			spriteBatch.End ();
            
			base.Draw (gameTime);
		}
	}
}

