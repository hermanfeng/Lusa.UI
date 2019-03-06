using System.Collections.Generic;
using System.Windows;
using AddinEngine;
using UIShell.OSGi;
using System.Linq;
using System.Collections.Specialized;
using WorkBenchContract;

namespace MainPanelPlugin
{
    public class DefaultMenuItemDescriptorProvider : MenuDescriptorProvider<MenuItemDescriptor>
    {
        public MenuItemDescriptor MenuItem { get; set; }

        public override MenuItemDescriptor Item
        {
            get { return MenuItem; }
        }
    }
}
