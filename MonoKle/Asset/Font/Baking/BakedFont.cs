using System.Collections.Generic;

namespace MonoKle.Asset.Font.Baking
{
    public class BakedFont
    {
        public List<byte[]> ImageList { get; set; }
        public FontFile FontFile { get; set; }
    }
}