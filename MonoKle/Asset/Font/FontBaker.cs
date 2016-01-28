namespace MonoKle.Asset.Font
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Threading.Tasks;

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
            catch(Exception e)
            {
                this.ErrorMessage = "Could not open font-file for reading.";
                this.DetailedError = e.Message;
                return false;
            }
            
            List<Image> images = new List<Image>();
            foreach (FontPage p in fontFile.Pages)
            {
                try
                {
                    Image image = Image.FromFile(p.File);
                    images.Add(image);
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
            baked.ImageList = images;

            try
            {
                using (FileStream bakeStream = File.OpenWrite(outputPath))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(bakeStream, baked);
                }
            }
            catch(Exception e)
            {
                this.ErrorMessage = "Could not bake to output file: " + outputPath;
                this.DetailedError = e.Message;
                return false;
            }

            return true;
        }

        [Serializable()]
        public class BakedFont
        {
            public List<Image> ImageList { get; set; }
            public FontFile FontFile { get; set; }
        }
    }
}
