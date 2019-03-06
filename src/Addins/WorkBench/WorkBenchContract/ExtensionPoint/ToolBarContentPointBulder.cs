using System.Windows;
using Lusa.AddinEngine.ExtendsionPoint;

namespace Lusa.UI.WorkBenchContract.ExtensionPoint
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
