using AddinEngine;

namespace WorkBenchContract
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
