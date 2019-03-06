using Lusa.AddinEngine;
using Lusa.UI.WorkBenchContract;
using UIShell.OSGi;

namespace Lusa.UI.WorkBench
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
