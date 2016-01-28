namespace MonoKle.FontOven
{
    using Asset.Font;
    using Asset.Font.Baking;
    using System;

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

            if(output.EndsWith(".mfnt") == false)
            {
                output = output + ".mfnt";
            }

            FontBaker baker = new FontBaker();
            if (baker.Bake(input, output))
            {
                Console.WriteLine("Success!");
            }
            else
            {
                Console.WriteLine("Error: " + baker.ErrorMessage);
                if(detailed)
                {
                    Console.WriteLine("Details: " + baker.DetailedError);
                }
            }
        }

        private static void DisplayUsage()
        {
            Console.WriteLine("Usage: fontoven <inputPath> <outputPath> [--detailed]");
        }
    }
}