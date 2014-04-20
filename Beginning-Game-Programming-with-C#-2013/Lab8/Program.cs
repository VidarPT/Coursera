using System;

namespace Lab8
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			// ask for string
			Console.Write ("Enter a string (number,letter,true_or_false): ");
			// string pyramid = Console.ReadLine ();
			string pyramid = ("15,M,true");
			Console.WriteLine (pyramid);

			// locate first and second comma
			int firstComma = pyramid.IndexOf (",");
			int secondComma = pyramid.IndexOf (",", firstComma + 1);

			// locate and print pyramid slot number
			int slotNumber = int.Parse (pyramid.Substring (0, firstComma));
			Console.WriteLine ("Slot number is: " + slotNumber);

			// locate and print pyramid block letter
			char blockLetter = char.Parse (pyramid.Substring ((firstComma + 1), 1 ));
			Console.WriteLine ("Block letter is: " + blockLetter);

			// locate and print whether or not the block is lit
			bool isLit = bool.Parse (pyramid.Substring (secondComma + 1));
			Console.WriteLine ("The block is lit: " + isLit);
		}
	}
}
