using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleCards
{
    /// <summary>
    /// A class for a playing card
    /// </summary>
    public class Card
    {
        #region Fields

        Rank rank;
        Suit suit;
        bool faceUp;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a card with the given rank and suit
        /// </summary>
        /// <param name="rank">the rank</param>
        /// <param name="suit">the suit</param>
        public Card(Rank rank, Suit suit)
        {
            this.rank = rank;
            this.suit = suit;
            faceUp = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the card rank
        /// </summary>
        public string Rank
        {
            get { return rank.ToString(); }
        }

        /// <summary>
        /// Gets the card suit
        /// </summary>
        public string Suit
        {
            get { return suit.ToString(); }
        }

        /// <summary>
        /// Gets whether or not the card is face up
        /// </summary>
        public bool FaceUp
        {
            get { return faceUp; }
        }

        /// <summary>
        /// Gets the Blackjack value for the card
        /// </summary>
        public int BlackjackValue
        {
            get
            {
                switch (rank)
                {
                    case ConsoleCards.Rank.Ace:
                        return 11;
                    case ConsoleCards.Rank.King:
                    case ConsoleCards.Rank.Queen:
                    case ConsoleCards.Rank.Jack:
                    case ConsoleCards.Rank.Ten:
                        return 10;
                    case ConsoleCards.Rank.Nine:
                        return 9;
                    case ConsoleCards.Rank.Eight:
                        return 8;
                    case ConsoleCards.Rank.Seven:
                        return 7;
                    case ConsoleCards.Rank.Six:
                        return 6;
                    case ConsoleCards.Rank.Five:
                        return 5;
                    case ConsoleCards.Rank.Four:
                        return 4;
                    case ConsoleCards.Rank.Three:
                        return 3;
                    case ConsoleCards.Rank.Two:
                        return 2;
                    default:
                        return 0;
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Flips the card over
        /// </summary>
        public void FlipOver()
        {
            faceUp = !faceUp;
        }

        /// <summary>
        /// Prints the card
        /// </summary>
        public void Print()
        {
            if (FaceUp)
            {
                Console.WriteLine(Rank + " of " + Suit);
            }
            else
            {
                Console.WriteLine("Face down card");
            }
        }

        #endregion
    }
}
