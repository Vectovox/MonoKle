using System;
using MonoKle.Asset;

namespace MonoKle.FontOven
{
    /// <summary>
    /// Tool to bake MonoKle fonts using the output from the BMFont tool.
    /// https://www.angelcode.com/products/bmfont/
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                DisplayUsage();
                return;
            }

            string input = args[0];
            string output = args.Length > 1 ? args[1] : args[0];
            bool detailed = args.Length > 2 && args[2].Equals("--detailed");

            if (output.EndsWith(".mfnt") == false)
            {
                output += ".mfnt";
            }

            var baker = new FontBaker();

            if (baker.Bake(input, output))
            {
                System.Console.WriteLine("Success!");
            }
            else
            {
                System.Console.WriteLine("Error: " + baker.ErrorMessage);
                if (detailed)
                {
                    System.Console.WriteLine("Details: " + baker.DetailedError);
                }
            }
        }

        private static void DisplayUsage()
        {
            System.Console.WriteLine("Welcome to FontOven!");
            System.Console.WriteLine("");
            System.Console.WriteLine("## Requirements");
            System.Console.WriteLine("This tool uses the output from the BMFont tool" +
                " (see https://www.angelcode.com/products/bmfont/).");
            System.Console.WriteLine("Remember to select .png and .xml as output!");
            System.Console.WriteLine("");
            System.Console.WriteLine("## Usage");
            System.Console.WriteLine("> fontoven <inputPath> [outputPath] [--detailed]");
            System.Console.WriteLine("If outputPath is not provided the input path will be used," +
                " appending the new file extension.");
        }
    }
}
