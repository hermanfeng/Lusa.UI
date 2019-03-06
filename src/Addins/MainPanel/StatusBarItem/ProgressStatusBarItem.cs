using System.Runtime.Remoting.Messaging;
using System.Security.RightsManagement;
using CommonLibrary;
using MainPanelPlugin.View;
using UIShell.OSGi;
using AddinEngine;
using WorkBenchContract;

namespace MainPanelPlugin
{
    public class ProgressStatusBarItem : StatusBarItem
    {
        public ProgressStatusBarItem()
        {
            this.IsLeft = false;
            this.Content = new ProgressItem();
        }
    }
}
