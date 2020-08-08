using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Xml.Serialization;
using MonoKle.Utilities;

namespace MonoKle.Asset
{
    /// <summary>
    /// Bakes MonoKle fonts using data from the BMFont tool.
    /// https://www.angelcode.com/products/bmfont/
    /// </summary>
    public class FontBaker
    {
        public string ErrorMessage { get; private set; }

        public string DetailedError { get; private set; }

        public bool Bake(string fontPath, string outputPath)
        {
            FileInfo fontPathInfo = new FileInfo(fontPath);
            FontFile fontFile = new FontFile();

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

            var dataList = new List<byte[]>();
            var isr = new ImageSerializer(ImageFormat.Png);
            foreach (FontPage p in fontFile.Pages)
            {
                try
                {
                    var image = Image.FromFile(Path.Combine(fontPathInfo.DirectoryName, p.File));
                    dataList.Add(isr.ImageToBytes(image));
                }
                catch (Exception e)
                {
                    ErrorMessage = "Could not open texture for reading: " + p.File;
                    DetailedError = e.Message;
                    return false;
                }
            }

            var baked = new BakedFont();
            baked.FontFile = fontFile;
            baked.ImageList = dataList;

            try
            {
                using FileStream bakeStream = File.OpenWrite(outputPath);
                var xx = new XmlSerializer(typeof(BakedFont));
                xx.Serialize(bakeStream, baked);
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