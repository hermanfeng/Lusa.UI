using System.Windows.Input;

namespace Lusa.UI.WorkBenchContract.UI.Commands
{
    public class ViewCommands
    {
        private static RoutedUICommand _saveLayout;
        public static RoutedUICommand SaveLayout
        {
            get
            {
                if (_saveLayout == null)
                {
                    _saveLayout = new RoutedUICommand("SaveLayout", "SaveLayout", typeof(ViewCommands));
                }
                return _saveLayout;
            }
        }

        private static RoutedUICommand _loadLayout;
        public static RoutedUICommand LoadLayout
        {
            get
            {
                if (_loadLayout == null)
                {
                    _loadLayout = new RoutedUICommand("LoadLayout", "LoadLayout", typeof(ViewCommands));
                }
                return _loadLayout;
            }
        }

        private static RoutedUICommand _resetLayout;
        public static RoutedUICommand ResetLayout
        {
            get
            {
                if (_resetLayout == null)
                {
                    _resetLayout = new RoutedUICommand("ResetLayout", "ResetLayout", typeof(ViewCommands));
                }
                return _resetLayout;
            }
        }

        private static RoutedUICommand _saveSettings;
        public static RoutedUICommand SaveSettings
        {
            get
            {
                if (_saveSettings == null)
                {
                    _saveSettings = new RoutedUICommand("SaveSettings", "SaveSettings", typeof(ViewCommands));
                }
                return _saveSettings;
            }
        }

        private static RoutedUICommand _saveAsSettings;
        public static RoutedUICommand SaveAsSettings
        {
            get
            {
                if (_saveAsSettings == null)
                {
                    _saveAsSettings = new RoutedUICommand("SaveSettingsAs", "SaveSettingsAs", typeof(ViewCommands));
                }
                return _saveAsSettings;
            }
        }

        private static RoutedUICommand _loadSetings;
        public static RoutedUICommand LoadSetings
        {
            get
            {
                if (_loadSetings == null)
                {
                    _loadSetings = new RoutedUICommand("LoadSetings", "LoadSetings", typeof(ViewCommands));
                }
                return _loadSetings;
            }
        }

    }
}
