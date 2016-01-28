namespace MonoKle.Asset.Font.Baking
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;

    public class ImageSerializer
    {
        public ImageFormat ImageFormat { get; set; }

        public ImageSerializer(ImageFormat format)
        {
            this.ImageFormat = format;
        }

        public byte[] ImageToBytes(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, this.ImageFormat);
                return ms.ToArray();
            }
        }

        public Image BytesToImage(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return Image.FromStream(ms);
            }
        }
    }
}