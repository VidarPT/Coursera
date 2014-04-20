using System;
using ConsoleCards;

namespace ProgrammingAssignment3
{
	/// <summary>
	/// Main class.
	/// </summary>
	class MainClass
	{
		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name="args">The command-line arguments.</param>
		private static void Main (string[] args)
		{
			// assign variables and create objects
			Deck deck = new Deck ();
			BlackjackHand playerHand = new BlackjackHand ("Player");
			BlackjackHand dealerHand = new BlackjackHand ("Dealer");

			// print welcome message
			Console.WriteLine ("Welcome!");
			Console.WriteLine ("This program will play a single hand of Blackjack!");

			// shuffle the deck
			deck.Shuffle ();

			// deal 2 cards to the player and dealer
			// Card card = deck.TakeTopCard ();
			playerHand.AddCard (deck.TakeTopCard ());
			dealerHand.AddCard (deck.TakeTopCard ());
			playerHand.AddCard (deck.TakeTopCard ());
			dealerHand.AddCard (deck.TakeTopCard ());

			// make all player's cards face up
			playerHand.ShowAllCards ();

			// make dealer's first card face up
			dealerHand.ShowFirstCard ();

			// print both hands
			playerHand.Print ();
			dealerHand.Print ();

			// let player hit if they want to
			Console.Write ("Sir, would you like to hit? (y, n): ");
			// char choice = Console.ReadLine ()[0];
			char choice = 'y';
			if (choice == 'y') {
				Console.WriteLine (choice);
				playerHand.AddCard (deck.TakeTopCard ());
				playerHand.ShowAllCards ();
			}

			// make all dealer's card face up
			dealerHand.ShowAllCards ();

			// print both player's hand and the dealer's
			playerHand.Print ();
			dealerHand.Print ();

			// print the scores for both hands
			Console.WriteLine ("Your score is " + playerHand.Score +
				"\n" + "The delear's score is " + dealerHand.Score + "\n");
			if (playerHand.Score > 21) {
				Console.Write ("Shouldn't have hit");
			} else if (playerHand.Score > dealerHand.Score ||
				playerHand.Score == dealerHand.Score) {
				Console.Write ("You won! :D");
			} else {
				Console.Write ("Welp... I guess you lost.");
			}
		}
	}
}
