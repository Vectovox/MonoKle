﻿using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace MonoKle.FontOven
{
    /// <summary>
    /// Serializes <see cref="Image"/> instances to byte arrays.
    /// </summary>
    public class ImageSerializer
    {
        public ImageFormat ImageFormat { get; set; }

        public ImageSerializer(ImageFormat format)
        {
            ImageFormat = format;
        }

        public byte[] ImageToBytes(Image image)
        {
            using var ms = new MemoryStream();
            image.Save(ms, ImageFormat);
            return ms.ToArray();
        }

        public Image BytesToImage(byte[] bytes)
        {
            using var ms = new MemoryStream(bytes);
            return Image.FromStream(ms);
        }
    }
}
