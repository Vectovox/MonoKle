using MonoKle.Configuration;
using System;
using System.Configuration;

namespace MonoKle.Engine
{
    public static class ConfigData
    {
        [CVar]
        public static string Company => ConfigurationManager.AppSettings["company"] ?? "MonoKle";
        [CVar]
        public static string CompanyFull => ConfigurationManager.AppSettings["companyFull"] ?? "MonoKle XYZ";
        [CVar]
        public static string Product => ConfigurationManager.AppSettings["product"] ?? "Demo";
        [CVar]
        public static string ProductYear => ConfigurationManager.AppSettings["productYear"] ?? DateTime.Now.Year.ToString();
        [CVar]
        public static string ProductVersion => VersionOverride ?? ConfigurationManager.AppSettings["productVersion"] ?? "1.0.0";
        [CVar]
        public static string InternalVersion => InternalVersionOverride ?? ConfigurationManager.AppSettings["internalVersion"] ?? "1.0.0";

        public static string VersionOverride { get; set; } = null;
        public static string InternalVersionOverride { get; set; } = null;
    }
}
