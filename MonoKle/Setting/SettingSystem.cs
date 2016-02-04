namespace MonoKle.Setting
{
    using IO;
    using Logging;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Class for storing and loading settings.
    /// </summary>
    public class SettingSystem : AbstractFileLoader
    {
        /// <summary>
        /// The default setting file path.
        /// </summary>
        public const string DefaultSettingPath = "settings.ini";

        private Dictionary<string, ISettingInstance> settingDictionary = new Dictionary<string, ISettingInstance>();

        /// <summary>
        /// Binds the specified instance to the specified setting. Any existing values will be assigned the instance.
        /// </summary>
        /// <param name="instance">The instance to bind.</param>
        /// <param name="setting">The setting to register for.</param>
        public void Bind(ISettingInstance instance, string setting)
        {
            this.Bind(instance, setting, true);
        }

        /// <summary>
        /// Binds the specified instance to the specified setting.
        /// </summary>
        /// <param name="instance">The instance to bind.</param>
        /// <param name="setting">The setting to register for.</param>
        /// <param name="assignOld">If set to true, assigns any existing value to the bound instance.</param>
        public void Bind(ISettingInstance instance, string setting, bool assignOld)
        {
            if (settingDictionary.ContainsKey(setting))
            {
                object oldValue = this.GetValue(setting);
                this.settingDictionary[setting] = instance;

                if (assignOld)
                {
                    instance.SetValue(oldValue);
                }
            }
            else
            {
                this.settingDictionary.Add(setting, instance);
            }
        }

        /// <summary>
        /// Clears all settings and bound instances.
        /// </summary>
        public void Clear()
        {
            this.settingDictionary.Clear();
            Logger.Global.Log("Cleared settings", LogLevel.Debug);
        }

        /// <summary>
        /// Gets the specified setting value.
        /// </summary>
        /// <param name="setting">The setting to get.</param>
        /// <returns>Setting value.</returns>
        public object GetValue(string setting)
        {
            if (settingDictionary.ContainsKey(setting))
            {
                return this.settingDictionary[setting].GetValue();
            }

            Logger.Global.Log("Accessed setting not bound: " + setting, LogLevel.Warning);
            return null;
        }

        /// <summary>
        /// Loads the default settings from the default path: <see cref="SettingSystem.DefaultSettingPath"/>.
        /// </summary>
        /// <returns>True if default path contained a setting file; otherwise false.</returns>
        public bool LoadDefault()
        {
            FileLoadingResult result = this.LoadFile(SettingSystem.DefaultSettingPath);
            if (result.Failures != 0)
            {
                Logger.Global.Log("Could not load default setting file.", LogLevel.Warning);
            }
            return result.Successes > 0;
        }

        /// <summary>
        /// Removes the specified setting, including eventual bound instance.
        /// </summary>
        /// <param name="setting">The setting to remove.</param>
        /// <returns>True if a setting was removed; otherwise false.</returns>
        public bool Remove(string setting)
        {
            return this.settingDictionary.Remove(setting);
        }

        /// <summary>
        /// Sets the value of the specified setting.
        /// </summary>
        /// <param name="setting">The setting to set.</param>
        /// <param name="value">The value to assign.</param>
        public void SetValue(string setting, object value)
        {
            if (settingDictionary.ContainsKey(setting))
            {
                this.settingDictionary[setting].SetValue(value);
            }
            else
            {
                this.settingDictionary.Add(setting, new ValueSetting(value));
            }
        }

        protected override bool OperateOnFile(Stream fileStream, string filePath)
        {
            StreamReader sr = new StreamReader(fileStream);
            return this.InterpretText(sr.ReadToEnd());
        }

        private bool InterpretText(string text)
        {
            // TODO: Add logic here.
            return true;
        }

        private class ValueSetting : ISettingInstance
        {
            private object value;

            public ValueSetting(object value)
            {
                this.value = value;
            }

            public object GetValue()
            {
                return this.value;
            }

            public void SetValue(object value)
            {
                this.value = value;
            }
        }
    }
}