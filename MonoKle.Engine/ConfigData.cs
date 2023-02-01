using MonoKle.Configuration;
using System;
using System.Configuration;

namespace MonoKle.Engine
{
    public static class ConfigData
    {
        [CVar("company")]
        public static string Company => ConfigurationManager.AppSettings["company"] ?? "MonoKle";
        [CVar("companyFull")]
        public static string CompanyFull => ConfigurationManager.AppSettings["companyFull"] ?? "MonoKle XYZ";
        [CVar("product")]
        public static string Product => ConfigurationManager.AppSettings["product"] ?? "Demo";
        [CVar("productYear")]
        public static string ProductYear => ConfigurationManager.AppSettings["productYear"] ?? DateTime.Now.Year.ToString();
        [CVar("productVersion")]
        public static string ProductVersion => VersionOverride ?? ConfigurationManager.AppSettings["productVersion"] ?? "1.0.0";

        public static string VersionOverride { get; set; } = null;
    }
}
