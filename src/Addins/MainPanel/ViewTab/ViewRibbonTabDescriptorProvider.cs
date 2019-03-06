using System.Collections.Generic;
using System.Windows;
using AddinEngine;
using UIShell.OSGi;
using System.Linq;
using System.Collections.Specialized;
using WorkBenchContract;

namespace MainPanelPlugin
{
    [ElementOrder(900)]
    public class ViewRibbonTabDescriptorProvider : MenuDescriptorProvider<MenuTabDescriptor>
    {
        public override MenuTabDescriptor Item
        {
            get { return new MenuTabDescriptor("TView", "View"); }
        }
    }
}
