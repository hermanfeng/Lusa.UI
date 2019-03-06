using UIShell.OSGi;
using AddinEngine;
using WorkBenchContract;

namespace MainPanelPlugin
{
    public class BundleActivator : BundleActivatorBase
    {
        //public static object OutputPane { get; set; }

        protected override void StartCore(IBundleContext context)
        {
            // used to listener the message earlier
            //OutputPane = new Output();
        }

        protected override void StopCore(IBundleContext context)
        {
            
        }
    }
}
