using UIShell.OSGi;
using WorkBench;
using AddinEngine;
using WorkBenchContract;

namespace WorkBenchPlugin
{
    public class BundleActivator : BundleActivatorBase
    {
        Workbench w;
        protected override void StartCore(IBundleContext context)
        {
            w = new Workbench();
            context.AddService<IWorkBench>(w);
        }

        protected override void StopCore(IBundleContext context)
        {
            context.RemoveService<IWorkBench>(w);
        }
    }
}
