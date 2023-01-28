using MonoKle.Configuration;
using System.Configuration;

namespace MonoKle.Engine
{
    public static class ConfigData
    {
        [CVar("company")]
        public static string Company => ConfigurationManager.AppSettings["company"] ?? "MonoKle";
        [CVar("product")]
        public static string Product => ConfigurationManager.AppSettings["product"] ?? "Demo";
        [CVar("productYear")]
        public static string ProductYear => ConfigurationManager.AppSettings["productYear"] ?? "Year";
    }
}
