using System;
using System.Collections.Generic;
using clausewitz_savegame_tokenizer; 

namespace clausewitz_savegame_tokenizer_example
{
    class Program
    {
        static void Main(string[] args)
        {
            string sample = "pop={\n\tname=\"Testling\"\n\tjob=\"Test subject\"\n}";

            Console.WriteLine("Input string:");
            Console.WriteLine(sample);
            Console.WriteLine("+++ end of input string +++");

            List<string> tokens = Tokenizer.Tokenize(sample);

            Console.WriteLine("Tokens:");
            foreach (string s in tokens)
                Console.WriteLine(s);
            Console.WriteLine("+++ end of tokens list +++");

            Console.WriteLine("+++ my job here is done +++");
            Console.ReadLine();
            Console.WriteLine("+++ but you didn't do anything! +++");
        }
    }
}
