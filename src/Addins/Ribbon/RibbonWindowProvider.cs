using Lusa.UI.WorkBenchContract;

namespace Lusa.UI.Ribbon
{
    public class RibbonWindowProvider: IWorkBenchWindowProvider
    {

        System.Windows.Window IWorkBenchWindowProvider.MainWindow
        {
            get { return new RibbonWindow();}
        }
    }
}
