using System;
using MonoKle.Asset;

namespace MonoKle.FontOven
{
    /// <summary>
    /// Tool to bake MonoKle fonts using data from the BMFont tool.
    /// https://www.angelcode.com/products/bmfont/
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Program.DisplayUsage();
                return;
            }

            string input = args[0];
            string output = args[1];
            bool detailed = args.Length > 2 && args[2].Equals("--detailed");

            if (output.EndsWith(".mfnt") == false)
            {
                output = output + ".mfnt";
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

        private static void DisplayUsage() => System.Console.WriteLine("Usage: fontoven <inputPath> <outputPath> [--detailed]");
    }
}
