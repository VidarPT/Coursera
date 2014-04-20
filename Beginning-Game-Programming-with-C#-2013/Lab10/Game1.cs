#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using ExplodingTeddies;

#endregion
namespace Lab10
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
		Explosion explosion;

		public Game1 ()
		{
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";
			graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
			graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;                                    	            
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

			// create two teddybears and explosion objects
			bear0 = new TeddyBear (Content, WINDOW_WIDTH, WINDOW_HEIGHT,
				"teddybear0", 300, 200, new Vector2 (-5, 0));
			bear1 = new TeddyBear (Content, WINDOW_WIDTH, WINDOW_HEIGHT,
				"teddybear1", 500, 200, new Vector2 (5, 0));
			explosion = new Explosion (Content);   
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
			// update teddybears
			bear0.Update ();
			bear1.Update ();
			explosion.Update (gameTime);			
			base.Update (gameTime);

			// check for collision
			if (bear0.Active &&
			    bear1.Active &&
			    bear0.DrawRectangle.Intersects (bear1.DrawRectangle)) {
				//deactivate bears
				bear0.Active = false;
				bear1.Active = false;

				// play explosion
				Rectangle collisionRectangle = Rectangle.Intersect (
					bear0.DrawRectangle, bear1.DrawRectangle);
				explosion.Play (collisionRectangle.Center.X, 
					collisionRectangle.Center.Y);

			}
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.CornflowerBlue);
		
			// draw teddybears and explosion
			spriteBatch.Begin ();
			bear0.Draw (spriteBatch);
			bear1.Draw (spriteBatch);
			if (bear0.Active) {
				explosion.Draw (spriteBatch);
			}
			spriteBatch.End ();
			base.Draw (gameTime);
		}
	}
}

