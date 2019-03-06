using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIShell.OSGi;

namespace AddinEngine
{
    public class BundleActivatorBase : IBundleActivator
    {
        static IBundleContext context;
        public static IBundleContext BundleContext
        {
            get
            {
                return context;
            }
        }

        public static T GetService<T>()
        {
            return BundleRuntime.Instance.GetFirstOrDefaultService<T>();
        }

        public void Start(IBundleContext context)
        {
            BundleActivatorBase.context = context;
            StartCore(context);
        }

        protected virtual void StartCore(IBundleContext context)
        {
            
        }

        public void Stop(IBundleContext context)
        {
            StopCore(context);
        }

        protected virtual void StopCore(IBundleContext context)
        {
            
        }
    }
}
