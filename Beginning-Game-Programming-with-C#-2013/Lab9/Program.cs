using System;

namespace Lab9
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			// print a menu for the user
			Console.WriteLine ("Menu:");
			Console.WriteLine ();
			Console.WriteLine ("1 - New Game");
			Console.WriteLine ("2 - Load Game");
			Console.WriteLine ("3 - Options");
			Console.WriteLine ("4 - Quit");
			Console.WriteLine ();

			// prompt the user for an option and store as an integer
			Console.Write ("Choose an option: ");
			// int choice = int.Parse (Console.ReadLine ());
			int choice = 1;

			// print appropriate message using if statements
			if (choice == 1) {
				Console.WriteLine ("Loading new game...");
				Console.WriteLine ();
			} else if (choice == 2) {
				Console.WriteLine ("Loading saved game...");
				Console.WriteLine ();
			} else if (choice == 3) {
				Console.WriteLine ("Options menu.");
				Console.WriteLine ();
			} else if (choice == 4) {
				Console.WriteLine ("Quitting so soon?");
				Console.WriteLine ();
			} else {
				Console.WriteLine ("nubster");
				Console.WriteLine ();
			}

			// print appropriate message using switch statements
			switch (choice) {
				case 1:
					Console.WriteLine ("Loading new game...");
					Console.WriteLine ();
					break;
				case 2:
					Console.WriteLine ("Loading saved game...");
					Console.WriteLine ();
					break;
				case 3:
					Console.WriteLine ("Options menu.");
					Console.WriteLine ();
					break;
				case 4:
					Console.WriteLine ("Quitting so soon?");
					Console.WriteLine ();
					break;
				default:
					Console.WriteLine ("nubster");
					Console.WriteLine ();
					break;
			}

		}
	}
}
