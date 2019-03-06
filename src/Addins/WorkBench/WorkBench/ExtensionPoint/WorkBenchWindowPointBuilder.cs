using Lusa.AddinEngine.ExtendsionPoint;
using Lusa.UI.WorkBenchContract;

namespace Lusa.UI.WorkBench.ExtensionPoint
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
