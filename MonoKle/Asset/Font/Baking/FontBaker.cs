namespace MonoKle.Asset.Font.Baking
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Xml.Serialization;

    public class FontBaker
    {
        public string ErrorMessage { get; private set; }

        public string DetailedError { get; private set; }

        public bool Bake(string fontPath, string outputPath)
        {
            FontFile fontFile = null;

            try
            {
                using (FileStream fontFileStream = File.OpenRead(fontPath))
                {
                    fontFile = FontLoader.Load(fontFileStream);
                }
            }
            catch (Exception e)
            {
                this.ErrorMessage = "Could not open font-file for reading.";
                this.DetailedError = e.Message;
                return false;
            }

            List<byte[]> dataList = new List<byte[]>();
            ImageSerializer isr = new ImageSerializer(ImageFormat.Png);
            foreach (FontPage p in fontFile.Pages)
            {
                try
                {
                    Image image = Image.FromFile(p.File);
                    dataList.Add(isr.ImageToBytes(image));
                }
                catch (Exception e)
                {
                    this.ErrorMessage = "Could not open texture for reading: " + p.File;
                    this.DetailedError = e.Message;
                    return false;
                }
            }

            BakedFont baked = new BakedFont();
            baked.FontFile = fontFile;
            baked.ImageList = dataList;

            try
            {
                using (FileStream bakeStream = File.OpenWrite(outputPath))
                {
                    XmlSerializer xx = new XmlSerializer(typeof(BakedFont));
                    xx.Serialize(bakeStream, baked);
                }
            }
            catch (Exception e)
            {
                this.ErrorMessage = "Could not bake to output file: " + outputPath;
                this.DetailedError = e.Message;
                return false;
            }

            return true;
        }
    }
}