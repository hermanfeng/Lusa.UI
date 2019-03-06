using System.Windows;
using AddinEngine;

namespace WorkBenchContract
{
    public class ToolBarContentPointBulder : ClassPointBuilder<FrameworkElement>
    {
        public const string ToolBarContentPoint = "WorkBench.ToolBarContentPoint";

        public override string ExensionPoint
        {
            get { return ToolBarContentPoint; }
        }
    }

    
}
