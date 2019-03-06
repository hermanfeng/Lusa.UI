using UIShell.OSGi;
using AddinEngine;
using CommonExtension;
using System;

namespace CommonLibrary
{
    public class Activator : BundleActivatorBase
    {
        protected override void StartCore(IBundleContext context)
        {
            base.StartCore(context);
            MessageService.Instance.SendMessage("CommonPlugin Started");
            context.FrameworkStateChanged += context_FrameworkStateChanged;
        }

        void context_FrameworkStateChanged(object sender, FrameworkEventArgs e)
        {
            if(e.EventType == FrameworkEventType.Error)
            {
                e.Data.As<Exception>(ex => MessageService.Instance.SendMessage(ex));
            }
        }
    }
}
