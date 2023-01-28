using MonoKle.Configuration;
using System;
using System.IO;

namespace MonoKle.Engine
{
    public static class GameDataStorage
    {
        private static Environment.SpecialFolder _root = Environment.SpecialFolder.LocalApplicationData;
        private static bool _dirty = true;

        private static string _cachedProjectPath;
        private static string _cachedLogsPath;

        public static Environment.SpecialFolder Root { get => _root; set { _root = value; _dirty = true; } }

        [CVar("projectDataPath")]
        public static string ProjectPath { get => GetProjectPath(); }

        [CVar("projectLogsPath")]
        public static string LogsPath { get => GetLogsPath(); }

        public static string GetProjectPath()
        {
            VerifyCache();
            return _cachedProjectPath;
        }

        public static string GetLogsPath()
        {
            VerifyCache();
            return _cachedLogsPath;
        }

        /// <summary>
        /// Creates the required folders to contain the game data. Does nothing for pre-existing folders.
        /// </summary>
        public static void CreateFolders()
        {
            Directory.CreateDirectory(GetProjectPath());
            Directory.CreateDirectory(GetLogsPath());
        }

        public static FileInfo GetLogFile(string name) => new(Path.Combine(GetLogsPath(), name));

        private static void VerifyCache()
        {
            if (_dirty)
            {
                _dirty = false;

                var root = Environment.GetFolderPath(Root);
                if (string.IsNullOrWhiteSpace(root))
                {
                    throw new InvalidOperationException($"Root path not accessible ({Root})!");
                }

                _cachedProjectPath = Path.Combine(Path.Combine(root, ConfigData.Company), ConfigData.Product);
                _cachedLogsPath = Path.Combine(_cachedProjectPath, "Logs");
            }
        }
    }
}
