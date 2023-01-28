using MonoKle.Configuration;
using System;
using System.IO;

namespace MonoKle.Engine
{
    public static class GameDataStorage
    {
        private static Environment.SpecialFolder _root = Environment.SpecialFolder.LocalApplicationData;
        private static string _companyName = "MonoKle";
        private static string _projectName = "Demo";
        private static readonly string _logFolderName = "Logs";
        private static bool _dirty = true;

        private static string _cachedProjectPath;
        private static string _cachedLogsPath;

        public static Environment.SpecialFolder Root { get => _root; set { _root = value; _dirty = true; } }
        public static string Company { get => _companyName; set { _companyName = value; _dirty = true; } }
        public static string Project { get => _projectName; set { _projectName = value; _dirty = true; } }

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

        public static FileInfo GetLogFile(string name) => new FileInfo(Path.Combine(GetLogsPath(), name));

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

                _cachedProjectPath = Path.Combine(Path.Combine(root, Company), Project);
                _cachedLogsPath = Path.Combine(_cachedProjectPath, _logFolderName);
            }
        }
    }
}
