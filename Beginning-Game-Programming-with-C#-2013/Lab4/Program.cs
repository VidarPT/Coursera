using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab4
{
    class Program
    {
		Deck myDeck;
        static void Main(string[] args)
        {
            // create a new deck and print the contents of the deck
			// Deck myDeck;
			myDeck = new Deck ();
			myDeck.Print ();
			Console.WriteLine ();

            // shuffle the deck and print the contents of the deck
			Console.WriteLine ("> Now we shuffle:");
			myDeck.Shuffle ();
			myDeck.Print ();
			Console.WriteLine ();

            // take the top card from the deck and print the card rank and suit
			Console.WriteLine ("> Top card:");
			Card card; 
			card = myDeck.TakeTopCard ();
			Console.WriteLine (card.Rank + " of " + card.Suit + "\n");

            // take the top card from the deck and print the card rank and suit
			Console.WriteLine ("> Another top card:");
			card = myDeck.TakeTopCard ();
			Console.WriteLine (card.Rank + " of " + card.Suit);

        }
    }
}
