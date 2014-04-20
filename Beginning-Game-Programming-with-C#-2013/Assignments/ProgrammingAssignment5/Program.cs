using System;

namespace ProgrammingAssignment5
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			// declare varible random and random object
			Random random = new Random();

			// declare variables for players rolls and wins and
			// variable wether to play again or not
			int player1Roll;
			int player2Roll;
			int player1Wins;
			int player2Wins;
			char playAgain = 'y';

			// print welcome message
			Console.WriteLine ("Welcome!");
			Console.WriteLine ("This program will play games of War.");

			// while the player wants to play again
			while (playAgain != 'n'&& 
				playAgain != 'N') {
				// set both player win counts to 0
				player1Wins = 0;
				player2Wins = 0;

				// loop for 21 battles
				for (int i = 0; i < 21; i++) {
					// roll the dice
					player1Roll = random.Next (1, 14);
					player2Roll = random.Next (1, 14);

					// check for war and roll again
					while (player1Roll == player2Roll) {
						Console.WriteLine ("   WAR!\tP1:" + player1Roll +
						"\tP2:" + player2Roll);
						player1Roll = random.Next (1, 14);
						player2Roll = random.Next (1, 14);
					}

					// print roll values
					Console.Write("BATTLE:\tP1:" + player1Roll +
						"\tP2:" + player2Roll + "\t");

					// print which player won and increment player's count
					if (player1Roll > player2Roll) {
						Console.WriteLine ("P1 Wins!");
						player1Wins++;
					} else if (player2Roll > player1Roll) {
						Console.WriteLine ("P2 Wins!");
						player2Wins++;
					}

				}

				// print overall winner
				Console.WriteLine ();
				if (player1Wins > player2Wins) {
					Console.WriteLine ("P1 is the overall winner with " +
						player1Wins + " battles!");
				} else {
					Console.WriteLine ("P2 is the overall winner with " +
						player1Wins + " battles!");
				}

                // ask for playing again
				Console.Write ("Would you like to play again (y, n)? ");
				playAgain = Console.ReadLine ()[0];
				Console.WriteLine ();
			}
		}
	}
}
