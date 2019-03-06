using System.Windows;
using Lusa.AddinEngine.ExtendsionPoint;

namespace Lusa.UI.WorkBenchContract.ExtensionPoint
{
    public class MainPanelContentPointBulder : ClassPointBuilder<FrameworkElement>
    {
        public const string MainPanelContentPoint = "WorkBench.MainPanelContentPoint";

        public override string ExensionPoint
        {
            get { return MainPanelContentPoint; }
        }
    }
}
