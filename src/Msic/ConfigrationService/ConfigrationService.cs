using AddinEngine;
using System;
using System.Configuration;
using System.Linq;
using CommonExtension;
using System.Collections.Generic;
using System.IO;

namespace CommonLibrary
{
    public class ConfigurationService : IConfigurationService
    {
        private static IConfigurationService service;
        public static IConfigurationService Instance
        {
            get
            {
                if (service.IsNull())
                {
                    service = new ConfigurationService();
                }
                return service;
            }
        }

        string IConfigurationService.GetConfigItem(string name)
        {
            var config = GetConfig();
            if (config.AppSettings.Settings.AllKeys.Contains(name))
            {
                return config.AppSettings.Settings[name].Value;
            }
            else
            {
                //load in default config
                var dconfig = GetConfig(DefaultName);
                if (dconfig.AppSettings.Settings.AllKeys.Contains(name))
                {
                    return dconfig.AppSettings.Settings[name].Value;
                }
            }
            return string.Empty;
        }

        void IConfigurationService.SetConfigItem(string name, string value)
        {
            if (name.IsNotNullOrEmpty())
            {
                Configuration config = GetConfig();
                AppSettingsSection app = config.AppSettings;
                app.Settings[name].Value = value;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }

        Dictionary<string, Configuration> cachedConfigs = new Dictionary<string, Configuration>();
        private Configuration GetConfig()
        {
            var configName = ((IConfigurationService)this).CurrentConfigName;
            return GetConfig(configName);

        }

        private Configuration GetConfig(string configName)
        {
            Configuration config = null;

            if (cachedConfigs.ContainsKey(configName))
            {
                return cachedConfigs[configName];
            }
            else
            {
                if (configName.IsNotNullOrEmpty())
                {
                    config = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap() { ExeConfigFilename = "config/" + configName + @".config" }, ConfigurationUserLevel.None);
                }
                if (config.IsNull() || !config.HasFile)
                {
                    config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                }
                cachedConfigs.Add(configName, config);
            }
            return config;
        }

        //private string BuildUnprotectedConfigFile(string configName)
        //{
        //   string path = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        //}

        private string currentConfigName = string.Empty;


        string IConfigurationService.CurrentConfigName
        {
            get
            {
                if(currentConfigName.IsNullOrEmpty())
                {
                    return DefaultConfigName;
                }
                return currentConfigName;
            }
            set
            {
                if (value != currentConfigName)
                {
                    currentConfigName = value;
                    if(ConfigChanged!=null)
                    {
                        ConfigChanged(this, new EventArgs());
                    }
                }
            }
        }

        bool defaultConfigLoaded;
        string defaultConfigName = string.Empty;
        private string DefaultConfigName
        {
            get
            {
                if(!defaultConfigLoaded)
                {
                    defaultConfigName = DefaultName;
                    EnsureLoadDefaultConfigName();
                }
                return defaultConfigName;
            }
        }
        private void EnsureLoadDefaultConfigName()
        {
            var key = @"config:";
            var config = Environment.GetCommandLineArgs().FirstOrDefault(line => line.StartsWith(key, StringComparison.OrdinalIgnoreCase));
            if(config.IsNotNullOrEmpty() && config.Length>key.Length)
            {
               defaultConfigName = config.Substring(key.Length);
            }
            defaultConfigLoaded = true;
        }

        public event EventHandler ConfigChanged;

        public static string DefaultName = "Default";
        List<string> allconfigs = new List<string>();


        IEnumerable<string> IConfigurationService.AllConfigs
        {
            get 
            {
                if(allconfigs.Count == 0)
                {
                    var configPath = Path.Combine(Environment.CurrentDirectory, "Config");
                    var files = Directory.EnumerateFiles(configPath, @"*.config");
                    allconfigs = files.Select(name => Path.GetFileNameWithoutExtension(name)).ToList();
                    allconfigs.Insert(0, DefaultName);
                }
                return allconfigs;
            }
        }

        
    }
}
