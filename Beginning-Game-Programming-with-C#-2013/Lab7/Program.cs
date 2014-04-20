using System;

namespace Lab7
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			// prompt message asking for birth month
			Console.Write ("On what month were you born? ");
			string month = Console.ReadLine ();

			// prompt message asking for birth day
			Console.Write ("On what day were you born? ");
			// int day = int.Parse (Console.ReadLine ());
			int day = 20;

			// calculate day before
			// int daybefore = (day - 1);

			// print the values
			Console.WriteLine ("Your birthday is " + month + " " + day);
			Console.WriteLine ("You'll receive an email reminder on " +
				month + (day - 1));
		}
	}
}
