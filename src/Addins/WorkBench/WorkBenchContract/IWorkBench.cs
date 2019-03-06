using System.Windows;
using System.Windows.Controls;

namespace Lusa.UI.WorkBenchContract
{
    public interface IWorkBench
    {
        void Initialize(WorkBenchEnviroment env);
        void Run(WorkBenchStartupSetting setting);
    }

    public enum UIMode
    {
        StandAlone=0,
        Embed=1
    }

    public class WorkBenchEnviroment
    {
        Application _app;

        public Application HostApplication
        {
            get 
            {
                if (_app == null)
                {
                    return Application.Current;
                }
                return _app; 
            }
            set { _app = value; }
        }
    }

    public class WorkBenchStartupSetting
    {
        UIMode _mode = UIMode.StandAlone;

        public UIMode Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        public Panel StartupContainer { get; set; }

        public IStartupReporter StartupReporter { get; set; }
    }

    public interface IStartupReporter
    {
        int CurrentProgress { get; }
        void Report(int progress, string msg);
    }
}
