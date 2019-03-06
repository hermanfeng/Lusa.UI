using System.Windows;
using AddinEngine;

namespace WorkBenchContract
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
