using Lusa.UI.WorkBenchContract;

namespace LusaRibbon
{
    public class RibbonWindowProvider: IWorkBenchWindowProvider
    {

        System.Windows.Window IWorkBenchWindowProvider.MainWindow
        {
            get { return new RibbonWindow();}
        }
    }
}
