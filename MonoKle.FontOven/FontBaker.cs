using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Xml.Serialization;
using MonoKle.Asset;

namespace MonoKle.FontOven
{
    /// <summary>
    /// Bakes MonoKle fonts using the output from the BMFont tool.
    /// https://www.angelcode.com/products/bmfont/
    /// </summary>
    public class FontBaker
    {
        public string ErrorMessage { get; private set; } = string.Empty;

        public string DetailedError { get; private set; } = string.Empty;

        public bool Bake(string fontPath, string outputPath)
        {
            FileInfo fontPathInfo = new(fontPath);
            FontFile fontFile = new();

            // Open the font XML
            try
            {
                using FileStream fontFileStream = File.OpenRead(fontPathInfo.FullName);
                fontFile = FontLoader.Load(fontFileStream);
            }
            catch (Exception e)
            {
                ErrorMessage = "Could not open font-file for reading.";
                DetailedError = e.Message;
                return false;
            }

            // Read XML data and load the referenced image data
            var dataList = new List<byte[]>();
            var imageSerializer = new ImageSerializer(ImageFormat.Png);
            foreach (FontPage p in fontFile.Pages)
            {
                try
                {
                    var image = Image.FromFile(Path.Combine(fontPathInfo.DirectoryName, p.File));
                    dataList.Add(imageSerializer.ImageToBytes(image));
                }
                catch (Exception e)
                {
                    ErrorMessage = "Could not open texture for reading: " + p.File;
                    DetailedError = e.Message;
                    return false;
                }
            }

            // Serialized the baked font into the output file
            var baked = new BakedFont
            {
                FontFile = fontFile,
                ImageList = dataList
            };
            try
            {
                using FileStream bakeStream = File.OpenWrite(outputPath);
                var xmlSerializer = new XmlSerializer(typeof(BakedFont));
                xmlSerializer.Serialize(bakeStream, baked);
            }
            catch (Exception e)
            {
                ErrorMessage = "Could not bake to output file: " + outputPath;
                DetailedError = e.Message;
                return false;
            }

            return true;
        }
    }
}
