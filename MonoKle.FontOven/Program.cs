using System;
using System.IO;
using MonoKle.Asset;

namespace MonoKle.FontOven
{
    /// <summary>
    /// Tool to bake MonoKle fonts using the output from the BMFont tool.
    /// https://www.angelcode.com/products/bmfont/
    /// </summary>
    internal class Program
    {
        private const string _detailedFlag = "--detailed";
        private const string _bakedExtension = ".mfnt";

        private static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                DisplayUsage();
                return;
            }

            // Get the input path
            var inputPath = args[0];
            var inputFileInfo = new FileInfo(inputPath);

            // Get the output path. If none is provided, just use input path with a new extension
            string output = args.Length > 1 && !args[1].Equals(_detailedFlag)
                ? args[1]
                : inputFileInfo.FullName.Remove(inputFileInfo.FullName.Length - inputFileInfo.Extension.Length,
                    inputFileInfo.Extension.Length) + _bakedExtension;

            // If they forgot to provide the extension we help by adding it
            if (!output.EndsWith(_bakedExtension))
            {
                output += _bakedExtension;
            }

            // Get the detailed flag
            bool detailed = args.Length > 2 && args[2].Equals(_detailedFlag);

            var baker = new FontBaker();
            if (baker.Bake(inputPath, output))
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
            System.Console.WriteLine("This tool uses the output from the BMFont tool" +
                ", see https://www.angelcode.com/products/bmfont/, and bakes it into one single file" +
                " for consumption in MonoKle.");
            System.Console.WriteLine("");
            System.Console.WriteLine("Remember to select .png and .xml as output in BMFont before baking!");
            System.Console.WriteLine("");
            System.Console.WriteLine("## Usage");
            System.Console.WriteLine($"> fontoven <inputPath> [outputPath] [{_detailedFlag}]");
            System.Console.WriteLine("If outputPath is not provided the input path will be used," +
                " appending the new file extension.");
        }
    }
}
