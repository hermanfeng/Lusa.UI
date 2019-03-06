using System.Collections.Generic;
using System.Windows;
using AddinEngine;
using UIShell.OSGi;
using System.Linq;
using System.Collections.Specialized;
using WorkBenchContract;

namespace MainPanelPlugin
{
    [ElementOrder(100)]
    public class PaneViewRibbonGroupDescriptorProvider : MenuDescriptorProvider<MenuGroupDescriptor>
    {
        public override MenuGroupDescriptor Item
        {
            get { return new MenuGroupDescriptor("TView.PaneView", "TView", "PaneView"); }
        }
    }
}
