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

using XnaCards;

namespace ProgrammingAssignment6
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // keep track of game state and current winner
        static GameState gameState = GameState.Play;
		Player currentWinner = Player.None;

        // hands and battle piles for the players
		WarHand player1Hand;
		WarHand player2Hand;
		WarBattlePile player1Pile;
		WarBattlePile player2Pile;

        // winner messages for players
		WinnerMessage player1Win;
		WinnerMessage player2Win;

        // menu buttons
		MenuButton flipButton;
		MenuButton collectWinningsButton;
		MenuButton quitButton;
 
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // make mouse visible and set resolution
			graphics.PreferredBackBufferWidth = 800;
			graphics.PreferredBackBufferHeight = 600;
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
            // TODO: Add your initialization logic here

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

            // create the deck object and shuffle
			Deck deck = new Deck (Content, 0, 0);
			deck.Shuffle ();

            // create the player hands and fully deal the deck into the hands
			player1Hand = new WarHand (graphics.PreferredBackBufferWidth / 2,
				graphics.PreferredBackBufferHeight / 6);
			player2Hand = new WarHand (graphics.PreferredBackBufferWidth / 2,
				graphics.PreferredBackBufferHeight * 5 / 6);
			while (!deck.Empty) {
				player1Hand.AddCard (deck.TakeTopCard ());
				player2Hand.AddCard (deck.TakeTopCard ());
			}

            // create the player battle piles
			player1Pile = new WarBattlePile (graphics.PreferredBackBufferWidth / 2,
				graphics.PreferredBackBufferHeight / 3);
			player2Pile = new WarBattlePile (graphics.PreferredBackBufferWidth / 2,
				graphics.PreferredBackBufferHeight * 2 / 3);

            // create the player winner messages
			player1Win = new WinnerMessage (Content,
				graphics.PreferredBackBufferWidth * 3 / 4,
				graphics.PreferredBackBufferHeight / 6);
			player2Win = new WinnerMessage (Content,
				graphics.PreferredBackBufferWidth * 3 / 4,
				graphics.PreferredBackBufferHeight * 5 / 6);

            // create the menu buttons
			flipButton = new MenuButton (Content, "flipbutton",
				graphics.PreferredBackBufferWidth / 5,
				graphics.PreferredBackBufferHeight / 5,
				GameState.Flip);
			collectWinningsButton = new MenuButton (Content, "collectwinningsbutton",
				graphics.PreferredBackBufferWidth / 5, 
				graphics.PreferredBackBufferHeight / 2,
				GameState.Play);
			collectWinningsButton.Visible = false;
			quitButton = new MenuButton (Content, "quitbutton", 
				graphics.PreferredBackBufferWidth / 5,
				graphics.PreferredBackBufferHeight * 4 / 5,
				GameState.Quit);

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
                this.Exit();

            // update the menu buttons
			MouseState mouse = Mouse.GetState ();
			flipButton.Update (mouse);
			collectWinningsButton.Update (mouse);
			quitButton.Update (mouse);
 
            // update based on game state
			if (gameState == GameState.Flip) {
				Card player1Card = player1Hand.TakeTopCard ();
				player1Card.FlipOver ();
				player1Pile.AddCard (player1Card);

				Card player2Card = player2Hand.TakeTopCard ();
				player2Card.FlipOver ();
				player2Pile.AddCard (player2Card);
	
				// figure out winner and show message
				if (player1Card.WarValue > player2Card.WarValue) {
					player1Win.Visible = true;
					currentWinner = Player.Player1;
				} else if (player2Card.WarValue > player1Card.WarValue) {
					player2Win.Visible = true;
					currentWinner = Player.Player2;
				} else {
					currentWinner = Player.None;
				}

				// adjust button visibility
				flipButton.Visible = false;
				collectWinningsButton.Visible = true;

                // wait to collecte winnings
				gameState = GameState.CollectWinnings;


			} else if (gameState == GameState.Play) {
				// distribute piles into appropriate hands and hide message
				if (currentWinner == Player.Player1) {
					player1Hand.AddCards (player1Pile);
					player1Hand.AddCards (player2Pile);
					player1Win.Visible = false;
				} else if (currentWinner == Player.Player2) {
					player2Hand.AddCards (player1Pile);
					player2Hand.AddCards (player2Pile);
					player2Win.Visible = false;
				} else {
					player1Hand.AddCards (player1Pile);
					player2Hand.AddCards (player2Pile);
				}
				currentWinner = Player.None;

                // adjust button visibility
				flipButton.Visible = true;
				collectWinningsButton.Visible = false;

				// check for game over
//				if (player1Hand.Empty) {
//					player2Win.Visible = true;
//					flipButton.Visible = false;
//					gameState = GameState.GameOver;
//				} else if (player2Hand.Empty) {
//					player1Win.Visible = true;
//					flipButton.Visible = false;
//					gameState = GameState.GameOver;
//				}
				if (player1Hand.Empty || player2Hand.Empty) {
					flipButton.Visible = false;
					gameState = GameState.GameOver;
					if (player1Hand.Empty) {
						player2Win.Visible = true;
					} else {
						player1Win.Visible = true;
					}
				}

			} else if (gameState == GameState.Quit) {
				Exit ();
			}
 
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Goldenrod);
			spriteBatch.Begin ();

            // draw the game objects
			player1Hand.Draw (spriteBatch);
			player2Hand.Draw (spriteBatch);
			player1Pile.Draw (spriteBatch);
			player2Pile.Draw (spriteBatch);

            // draw the winner messages
			player1Win.Draw (spriteBatch);
			player2Win.Draw (spriteBatch);

            // draw the menu buttons
			flipButton.Draw (spriteBatch);
		    collectWinningsButton.Draw (spriteBatch);
			quitButton.Draw (spriteBatch);

			spriteBatch.End ();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Changes the state of the game
        /// </summary>
        /// <param name="newState">the new game state</param>
        public static void ChangeState(GameState newState)
        {
            gameState = newState;
        }
    }
}
