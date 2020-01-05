namespace MonoKle.Asset.Font.Baking
{
    using System.Collections.Generic;

    public class BakedFont
    {
        public List<byte[]> ImageList { get; set; }
        public FontFile FontFile { get; set; }
    }
}