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
            ImageFormat = format;
        }

        public byte[] ImageToBytes(Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat);
                return ms.ToArray();
            }
        }

        public Image BytesToImage(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                return Image.FromStream(ms);
            }
        }
    }
}
