using System.Windows;
using UIShell.OSGi;
using WorkBenchContract;

namespace AddinEngine
{
    public class WorkBenchWindowPointBuilder : ClassPointBuilder<IWorkBenchWindowProvider>
    {
        public const string GlobalResourcesPoint = "workbench.WorkBenchWindowProvider";

        public override string ExensionPoint
        {
            get { return GlobalResourcesPoint; }
        }
    }

}
