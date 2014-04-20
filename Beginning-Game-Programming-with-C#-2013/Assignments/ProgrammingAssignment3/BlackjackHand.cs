using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleCards
{
    /// <summary>
    /// A class for a Blackjack hand
    /// </summary>
    public class BlackjackHand
    {
        #region Fields

        const int MAX_HAND_VALUE = 21;

        string ownerName;
        List<Card> cards = new List<Card>();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ownerName">the name of the player owning the hand</param>
        public BlackjackHand(string ownerName)
        {
            this.ownerName = ownerName;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the score for the hand
        /// </summary>
        public int Score
        {
            get
            {
                // add up score excluding Aces
                int numAces = 0;
                int score = 0;
                foreach (Card card in cards)
                {
                    if (card.Rank != Rank.Ace.ToString())
                    {
                        score += card.BlackjackValue;
                    }
                    else
                    {
                        numAces++;
                    }
                }

                // if more than one ace, only one should ever be counted as 11
                if (numAces > 1)
                {
                    // make all but the first ace count as 1
                    score += numAces - 1;
                    numAces = 1;
                }

                // if there's an Ace,score it the best way possible
                if (numAces > 0)
                {
                    if (score + 11 <= MAX_HAND_VALUE)
                    {
                        // counting Ace as 11 doesn't bust
                        score += 11;
                    }
                    else
                    {
                        // count Ace as 1
                        score++;
                    }
                }

                return score;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds a card to the hand
        /// </summary>
        /// <param name="card">the card to add</param>
        public void AddCard(Card card)
        {
            cards.Add(card);
        }

        /// <summary>
        /// Lets a player decide whether or not to hit using console input
        /// </summary>
        /// <param name="deck">the deck to potentially hit from</param>
        public void HitOrNot(Deck deck)
        {
            if (Hit())
            {
                Card card = deck.TakeTopCard();
                if (!card.FaceUp)
                {
                    card.FlipOver();
                }
                Console.Write("Your new card is the ");
                card.Print();
                Console.WriteLine();
                cards.Add(card);
            }
        }

        /// <summary>
        /// Shows the first card in the hand by making it face up
        /// </summary>
        public void ShowFirstCard()
        {
            if (cards.Count > 0 &&
                !cards[0].FaceUp)
            {
                cards[0].FlipOver();
            }
        }

        /// <summary>
        /// Shows all the cards in the hand by making them face up
        /// </summary>
        public void ShowAllCards()
        {
            foreach (Card card in cards)
            {
                if (!card.FaceUp)
                {
                    card.FlipOver();
                }
            }
        }

        /// <summary>
        /// Prints the cards in the hand
        /// </summary>
        public void Print()
        {
            string headerName = ownerName + "'s Hand";
            Console.WriteLine(headerName);
            Console.WriteLine(GetDashesString(headerName.Length));
            foreach (Card card in cards)
            {
                card.Print();
            }
            Console.WriteLine();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Gets a string of dashes of the given length
        /// </summary>
        /// <param name="length">the length of the string</param>
        /// <returns>the string of dashes</returns>
        private string GetDashesString(int length)
        {
            StringBuilder dashes = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                dashes.Append('-');
            }
            return dashes.ToString();
        }

        /// <summary>
        /// Tells whether or not the player wants to hit
        /// </summary>
        /// <returns>whether to hit or not</returns>
        private bool Hit()
        {
            char hitChar = 'z';

            // prompt for and get hit or not decision
            bool valid = false;
            while (!valid)
            {
                // prompt for and get player choice
                Console.Write("Would you like to hit (y, n)? ");
                string choice = Console.ReadLine();

                // print error message as necessary
                if (choice.Length > 0)
                {
                    hitChar = Char.ToLower(choice[0]);
                }
                else
                {
                    hitChar = 'z';
                }
                if (choice.Length > 1 ||
                    (hitChar != 'y' && hitChar != 'n'))
                {
                    hitChar = 'z';
                    Console.WriteLine("You need to enter a single character, y for yes or n for no!");
                    Console.WriteLine();
                }
                valid = hitChar == 'y' || hitChar == 'n';
            }

            Console.WriteLine();
            return hitChar == 'y';
        }

        #endregion
    }
}
