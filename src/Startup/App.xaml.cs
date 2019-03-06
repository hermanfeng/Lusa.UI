using System;
using System.Configuration;
using System.Diagnostics;
using System.Windows;
using CommonLibrary;
using UIShell.iOpenWorks.Bootstrapper;
using UIShell.OSGi;
using UIShell.OSGi.Logging;
using UIShell.OSGi.Utility;
using AddinEngine;
using WorkBenchContract;

namespace AppStartUp
{
    /// <summary>
    /// WPF startup class.
    /// </summary>
    public partial class App : Application,IStartupReporter
    {
        private BundleRuntime _bundleRuntime;
        // Use object type to avoid load UIShell.OSGi.dll before update.

        [STAThreadAttribute]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public static void Main()
        {
            var app = new App();
            app.Start();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            //SplashWindow.Instance.Close();
            base.OnStartup(e);
        }

        private void Start()
        {
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-CN");
            //UpdateCore();
            StartBundleRuntime();
        }

        void UpdateCore() // Update Core Files, including BundleRepositoryOpenAPI, PageFlowService and OSGi Core assemblies.
        {
            if (AutoUpdateCoreFiles)
            {
                new CoreFileUpdater().UpdateCoreFiles(CoreFileUpdateCheckType.Daily);
            }
        }


        void StartBundleRuntime() // Start OSGi Core.
        {
            FileLogUtility.SetLogLevel(LogLevel);
            FileLogUtility.SetMaxFileSizeByMB(MaxLogFileSize);
            FileLogUtility.SetCreateNewFileOnMaxSize(CreateNewLogFileOnMaxSize);
            var st = new Stopwatch();
            st.Start();
            var setting = new AddinEngineStartUpSetting();
            AddinEngineHost.InitializeBundleRuntime(setting);

            SplashWindow.Instance.Show();

            var bundleRuntime = AddinEngineHost.Runtime;
            bundleRuntime.AddService<Application>(this);
            AddinEngineHost.StartRuntime();

            Exit += AppExit;
            _bundleRuntime = bundleRuntime;
            st.Stop();
            MessageService.Instance.SendMessage("StartRuntime takes "+st.ElapsedMilliseconds+"ms");
            StartupWorkbench();
        }

        void StartupWorkbench()
        {
            var bundleRuntime = AddinEngineHost.Runtime;
            Application app = Application.Current;
            app.DispatcherUnhandledException += app_DispatcherUnhandledException;
            System.AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            app.ShutdownMode = ShutdownMode.OnLastWindowClose;
            var bench = bundleRuntime.GetFirstOrDefaultService<IWorkBench>();
            if (bench != null)
            {
                var env = new WorkBenchEnviroment();
                var startsetting = new WorkBenchStartupSetting();
                startsetting.StartupReporter = this;
                SplashWindow.Instance.SetProgress(70,"Initialize workbench.");
                bench.Initialize(env);
                SplashWindow.Instance.SetProgress(90, "Run workbench.");
                bench.Run(startsetting);
            }
            else
            {
                SplashWindow.Instance.SetProgress(90, "Can not find workbench.");
            }
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageService.Instance.SendMessage(e.ExceptionObject as Exception);
        }

        void app_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageService.Instance.SendMessage(e.Exception);
        }

        void AppExit(object sender, ExitEventArgs e)
        {
            if (_bundleRuntime != null)
            {
                var bundleRuntime = _bundleRuntime as BundleRuntime;
                bundleRuntime.Stop();
                _bundleRuntime = null;
            }
        }
        #region Settings
        /// <summary>
        /// 日志级别。
        /// </summary>
        private static LogLevel LogLevel
        {
            get
            {
                string level = ConfigurationManager.AppSettings["LogLevel"];
                if (!string.IsNullOrEmpty(level))
                {
                    try
                    {
                        object result = Enum.Parse(typeof(LogLevel), level);
                        if (result != null)
                        {
                            return (LogLevel)result;
                        }
                    }
                    catch (Exception)
                    { }
                }
                return LogLevel.Debug;
            }
        }

        /// <summary>
        /// 日志文件限制的大小。
        /// </summary>
        private static int MaxLogFileSize
        {
            get
            {
                string size = ConfigurationManager.AppSettings["MaxLogFileSize"];
                if (!string.IsNullOrEmpty(size))
                {
                    try
                    {
                        return int.Parse(size);
                    }
                    catch { }
                }

                return 10;
            }
        }

        /// <summary>
        /// 当日志大小超过限制时，是否新建一个。
        /// </summary>
        private static bool CreateNewLogFileOnMaxSize
        {
            get
            {
                string createNew = ConfigurationManager.AppSettings["CreateNewLogFileOnMaxSize"];
                if (!string.IsNullOrEmpty(createNew))
                {
                    try
                    {
                        return bool.Parse(createNew);
                    }
                    catch { }
                }

                return false;
            }
        }

        /// <summary>
        /// 当日志大小超过限制时，是否新建一个。
        /// </summary>
        private static bool AutoUpdateCoreFiles
        {
            get
            {
                string autoUpdateCoreFiles = ConfigurationManager.AppSettings["AutoUpdateCoreFiles"];
                if (!string.IsNullOrEmpty(autoUpdateCoreFiles))
                {
                    try
                    {
                        return bool.Parse(autoUpdateCoreFiles);
                    }
                    catch (Exception)
                    { }
                }

                return false;
            }
        }
        #endregion

        int IStartupReporter.CurrentProgress
        {
            get { return SplashWindow.Instance.CurrentProgress; }
        }

        void IStartupReporter.Report(int progress, string msg)
        {
            SplashWindow.Instance.SetProgress(progress,msg);
        }
    }
}
