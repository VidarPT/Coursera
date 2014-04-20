using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject
{
    /// <remarks>
    /// A number tile
    /// </remarks>
    class NumberTile
    {
        #region Fields

        // original length of each side of the tile
        int originalSideLength;

        // whether or not this tile is the correct number
        bool isCorrectNumber;

        // drawing support
        Texture2D texture;
		Texture2D blinkingTexture;
		Texture2D currentTexture;
        Rectangle drawRectangle;
        Rectangle sourceRectangle;

        // blinking support
        const int TOTAL_BLINK_MILLISECONDS = 4000;
        int elapsedBlinkMilliseconds = 0;
        const int FRAME_BLINK_MILLISECONDS = 1000;
        int elapsedFrameMilliseconds = 0;

		// shrinking support
		const int TOTAL_SHRINK_MILLISECONDS = 2000;
		int elapsedShrinkMilliseconds = 0;

		// tile state
		TileState tileState = new TileState();

		// clicking support
		bool clickStarted = false;
		bool buttonReleased = false;

		// sound support
		SoundEffect correctGuess;
		SoundEffect incorrectGuess;
		                 

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contentManager">the content manager</param>
        /// <param name="center">the center of the tile</param>
        /// <param name="sideLength">the side length for the tile</param>
        /// <param name="number">the number for the tile</param>
        /// <param name="correctNumber">the correct number</param>
		public NumberTile(ContentManager contentManager, Vector2 tileCenter, int sideLength,
            int number, int correctNumber)
        {
            // set original side length field
            this.originalSideLength = sideLength;

			// load sound effects
			correctGuess = contentManager.Load<SoundEffect> ("explosion");
			incorrectGuess = contentManager.Load<SoundEffect> ("loser");
            

            // load content for the tile and create draw rectangle
			LoadContent(contentManager, number);
			drawRectangle = new Rectangle(((int)tileCenter.X - sideLength / 2),
				((int)tileCenter.Y - sideLength / 2), sideLength, sideLength);

            // set isCorrectNumber flag
            isCorrectNumber = number == correctNumber;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Updates the tile based on game time and mouse state
        /// </summary>
        /// <param name="gameTime">the current GameTime</param>
        /// <param name="mouse">the current mouse state</param>
        /// <return>true if the correct number was guessed, false otherwise</return>
        public bool Update(GameTime gameTime, MouseState mouse)
        {
			// handle blinkin, shrinking or normal processing
			if (tileState == TileState.Blinking) {
				// check for finished blinking
				elapsedBlinkMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
				if (elapsedBlinkMilliseconds >= TOTAL_BLINK_MILLISECONDS) {
					tileState = TileState.Invisible;
					return true;
				} else {
					// check for frame change
					elapsedFrameMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
					if (elapsedFrameMilliseconds >= FRAME_BLINK_MILLISECONDS) {
						elapsedFrameMilliseconds = 0;
						if (sourceRectangle.X > 0) {
							sourceRectangle.X = 0;
						} else {
							sourceRectangle.X = currentTexture.Width / 2;
						}
					}
				}
			}
			else if (tileState == TileState.Shrinking) {
				elapsedShrinkMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
				int tileSideLenghth = (int)(originalSideLength *
				                      ((float)(TOTAL_SHRINK_MILLISECONDS - elapsedShrinkMilliseconds) /
				                      TOTAL_SHRINK_MILLISECONDS));
				if (tileSideLenghth > 0) {
					drawRectangle.Width = tileSideLenghth;
					drawRectangle.Height = tileSideLenghth;
				} else {
					tileState = TileState.Invisible;
				}
			} else {
				// highlight with mouse over tile
				if (drawRectangle.Contains (mouse.X, mouse.Y)) {
					// highlight number tile
					sourceRectangle.X = texture.Width / 2;

					// check for click on number tile
					if (mouse.LeftButton == ButtonState.Pressed &&
					   buttonReleased) {
						clickStarted = true;
						buttonReleased = false;
					} else if (mouse.LeftButton == ButtonState.Released) {
						buttonReleased = true;

						// if click finished on tile, change tile state
						if (clickStarted) {
							// check for blinking or shrinking
							if (isCorrectNumber) {
								tileState = TileState.Blinking;
								correctGuess.Play ();
								currentTexture = blinkingTexture;
								sourceRectangle.X = 0;
							} else {
								tileState = TileState.Shrinking;
								incorrectGuess.Play ();
							}

							clickStarted = false;
						}
					}

				} else {
					sourceRectangle.X = 0;

					// no clicking on this tile
					clickStarted = false;
					buttonReleased = false;
				}
			}

            // if we get here, return false
            return false;
        }

        /// <summary>
        /// Draws the number tile
        /// </summary>
        /// <param name="spriteBatch">the SpriteBatch to use for the drawing</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // draw the tile
			if (tileState != TileState.Invisible) {
				spriteBatch.Draw (currentTexture, drawRectangle,
					sourceRectangle, Color.White);
			}
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Loads the content for the tile
        /// </summary>
        /// <param name="contentManager">the content manager</param>
        /// <param name="number">the tile number</param>
        private void LoadContent(ContentManager contentManager, int number)
        {
            // convert the number to a string
            string numberString = ConvertIntToString(number);

            // load content for the tile and set source rectangle
			texture = contentManager.Load<Texture2D> (numberString);
			blinkingTexture = contentManager.Load<Texture2D> ("blinking" + numberString);
			currentTexture = texture;
			sourceRectangle = new Rectangle (0, 0, (currentTexture.Width / 2),
				currentTexture.Height);

        }

        /// <summary>
        /// Converts an integer to a string for the corresponding number
        /// </summary>
        /// <param name="number">the integer to convert</param>
        /// <returns>the string for the corresponding number</returns>
        private String ConvertIntToString(int number)
        {
            switch (number)
            {
                case 1:
                    return "one";
                case 2:
                    return "two";
                case 3:
                    return "three";
                case 4:
                    return "four";
                case 5:
                    return "five";
                case 6:
                    return "six";
                case 7:
                    return "seven";
                case 8:
                    return "eight";
                case 9:
                    return "nine";
                default:
                    throw new Exception("Unsupported number for number tile");
            }

        }

        #endregion
    }
}
