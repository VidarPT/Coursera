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

namespace GameProject
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		const int WINDOW_WIDTH = 800;
		const int WINDOW_HEIGHT = 600;

        // game state
		GameState gameState = GameState.Menu;

        // Increment 1: opening screen fields
		Texture2D openingScreen;
		Rectangle drawRectangle;

        // Increment 2: the board
		NumberBoard numberBoard;
		Vector2 center;
		int sideLength;

		// random number generator
		Random rand = new Random() ; 

		// new game sound effect
		SoundEffect newGameSoundEffect;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Increment 1: set window resolution
			graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
			graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;

            // Increment 1: make the mouse visible
			this.IsMouseVisible = true;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
             base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

			// load sound effect
			newGameSoundEffect = Content.Load<SoundEffect> ("applause");

            // Increment 1: load opening screen and set opening screen draw rectangle
			openingScreen = Content.Load<Texture2D> ("openingscreen");
			drawRectangle = new Rectangle ((WINDOW_WIDTH / 2 - openingScreen.Width / 2), 
				(WINDOW_HEIGHT / 2 - openingScreen.Height / 2), 
				800, 600);

			// call new game method
			StartGame ();
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
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
				Keyboard.GetState ().IsKeyDown (Keys.Escape))
                this.Exit();

            // Increment 2: change game state if game state is GameState.Menu and user presses Enter
			if (gameState == GameState.Menu &&
				Keyboard.GetState().IsKeyDown (Keys.Enter)) {
					gameState = GameState.Play;
				}
            // if we're actually playing, update mouse state and update board
			if (gameState == GameState.Play) {
				if (numberBoard.Update (gameTime, Mouse.GetState ())) {
					newGameSoundEffect.Play ();
					StartGame ();
				}
			}

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Increments 1 and 2: draw appropriate items here
			spriteBatch.Begin ();
			if (gameState == GameState.Menu) {
				spriteBatch.Draw (openingScreen, drawRectangle, Color.White);
			} else if (gameState == GameState.Play) {
				numberBoard.Draw (spriteBatch);
			}
			spriteBatch.End ();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Starts a game
        /// </summary>
        void StartGame()
        {
            // randomly generate new number for game
			int correctNumber = rand.Next (1, 10);

            // create the board object (this will be moved before you're done)
			center = new Vector2 ((WINDOW_WIDTH / 2), (WINDOW_HEIGHT / 2));
			sideLength = Convert.ToInt32 (Math.Min (WINDOW_WIDTH, WINDOW_HEIGHT) * 0.9);
			numberBoard = new NumberBoard (Content, center, sideLength, correctNumber);
        }
    }
}
