using System;
using System.Collections.Generic;

namespace Lusa.UI.Msic.ConfigrationService
{
    public interface IConfigurationService
    {
        string GetConfigItem(string name);
        void SetConfigItem(string name, string value);
        string CurrentConfigName { get; set; }
        event EventHandler ConfigChanged;
        IEnumerable<string> AllConfigs { get; }
    }

}
