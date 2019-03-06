using Lusa.AddinEngine.ExtendsionPoint;

namespace Lusa.UI.WorkBenchContract.ExtensionPoint
{
    public class WorkBenchInitializerPointBuilder : ClassPointBuilder<IWorkBenchInitializer>
    {
        public const string GlobalResourcesPoint = "workbench.WorkBenchInitializer";

        public override string ExensionPoint
        {
            get { return GlobalResourcesPoint; }
        }
    }
}
