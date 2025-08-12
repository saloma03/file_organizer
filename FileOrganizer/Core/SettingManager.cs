﻿using System.IO;
using System.Text.Json;

namespace FileOrganizer.Core
{
    public class SettingManager
    {

        private readonly string _settingsFilePath;

        public SettingManager(string settingsFilePath = "settings.json")
        {
            _settingsFilePath = settingsFilePath;
        }

        public List<Rule> LoadRules()
        {
            if (!File.Exists(_settingsFilePath))
            {
                var defaultRules = new List<Rule>
                {
                    new Rule { Extension = ".jpg", FolderName = "Images" },
                    new Rule { Extension = ".png", FolderName = "Images" },
                    new Rule { Extension = ".gif", FolderName = "Images" },
                    new Rule { Extension = ".pdf", FolderName = "Documents" },
                    new Rule { Extension = ".docx", FolderName = "Documents" },
                    new Rule { Extension = ".mp4", FolderName = "Videos" },
                    new Rule { Extension = ".mkv", FolderName = "Videos" },
                    new Rule { Extension = ".psd", FolderName = "Designs" } 
                };
                SaveRules(defaultRules);
                return defaultRules;
            }

            var json = File.ReadAllText(_settingsFilePath);
            return JsonSerializer.Deserialize<List<Rule>>(json) ?? new List<Rule>();
        }

        public void SaveRules(List<Rule> rules)
        {
            var options = new JsonSerializerOptions { WriteIndented = true }; 
            var json = JsonSerializer.Serialize(rules, options);
            File.WriteAllText(_settingsFilePath, json);
        }
    }

}
