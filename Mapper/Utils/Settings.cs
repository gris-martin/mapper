using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;

namespace Mapper.Utils
{
    class Settings
    {
        [JsonIgnore]
        public static string SettingsFilePath
        {
            get
            {
                var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var relativeSettingsPath = Application.Current.FindResource("RelativeSettingsPath") as string;
                return Path.Combine(localAppData, relativeSettingsPath);
            }
        }

        /// <summary>
        /// Path to the last saved file.
        /// </summary>
        public string LastSavePath { get; set; }

        /// <summary>
        /// Create a new Settings object from the default file path.
        /// The default file path is [LocalAppData]/Mapper/settings.json.
        /// </summary>
        /// <returns></returns>
        public static Settings FromFile()
        {
            if (!File.Exists(SettingsFilePath))
                return new Settings();

            var jsonString = File.ReadAllText(SettingsFilePath);
            return JsonSerializer.Deserialize<Settings>(jsonString);
        }

        /// <summary>
        /// Save the current settings
        /// </summary>
        public void Save()
        {
            var serializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var jsonString = JsonSerializer.Serialize(this, serializerOptions);
            var folder = Directory.GetParent(SettingsFilePath);
            Directory.CreateDirectory(folder.FullName);
            File.WriteAllText(SettingsFilePath, jsonString);
        }
    }
}
