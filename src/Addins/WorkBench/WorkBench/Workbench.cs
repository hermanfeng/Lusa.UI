using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using Lusa.AddinEngine.Extension;
using Lusa.UI.Msic.MessageService;
using Lusa.UI.WorkBench.ExtensionPoint;
using Lusa.UI.WorkBenchContract;

namespace Lusa.UI.WorkBench
{
    public class Workbench : IWorkBench
    {
        Application _app;

        void IWorkBench.Initialize(WorkBenchEnviroment env)
        {
#if DEBUG
            var st = new Stopwatch();
            st.Start();
#endif
            _app = env.HostApplication;
            _app.DispatcherUnhandledException += _app_DispatcherUnhandledException;
            var resbuild = new GlobalResourcesPointBuilder();

            resbuild.BuildItems().GeneratedItems.ForEach(
                res => _app.Resources.MergedDictionaries.Add(res));

            var resbuild1 = new GlobalResourcesLevel1PointBuilder();
            resbuild1.BuildItems().GeneratedItems.ForEach(
                res => _app.Resources.MergedDictionaries.Add(res));

            var resbuild2 = new GlobalResourcesLevel2PointBuilder();
            resbuild2.BuildItems().GeneratedItems.ForEach(
                res => _app.Resources.MergedDictionaries.Add(res));
#if DEBUG
            st.Stop();
            MessageService.Instance.SendMessage("IWorkBench.Initialize takes "+st.ElapsedMilliseconds+"ms");
#endif
        }

        void _app_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageService.Instance.SendMessage(e.Exception);
        }

        void IWorkBench.Run(WorkBenchStartupSetting setting)
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            if (setting.Mode == UIMode.StandAlone)
            {
#if DEBUG
                var st = new Stopwatch();
                st.Start();
#endif
                var windowBuilder = new WorkBenchWindowPointBuilder();
                var window = BuildCustomWindow(windowBuilder);
#if DEBUG
                st.Stop();
                MessageService.Instance.SendMessage("IWorkBench.Run BuildCustomWindow takes " + st.ElapsedMilliseconds + "ms");
                st.Reset();
                st.Start();
#endif
                if(window.IsNull())
                {
                    window = new WorkBenchMainWindow();
                }
                window.Show();
                setting.StartupReporter.As<IStartupReporter>(reporter => reporter.Report(100, "Startup completely."));
#if DEBUG
                st.Stop();
                MessageService.Instance.SendMessage("IWorkBench.Run WindowShow takes " + st.ElapsedMilliseconds + "ms");
#endif
                _app.Run(window);
            }
            else if(setting.Mode == UIMode.Embed)
            {
                if (setting.StartupContainer != null)
                {
                    var content = new EmbedContent();
                    setting.StartupContainer.Children.Add(content);
                }
            }
        }

        private static Window BuildCustomWindow(WorkBenchWindowPointBuilder windowBuilder)
        {
            var windowProvider = windowBuilder.BuildItems().GeneratedItems.FirstOrDefault();
            if (windowProvider != null)
            {
                return windowProvider.MainWindow;
            }
            return null;
        }
    }
}
