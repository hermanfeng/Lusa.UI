using Lusa.AddinEngine.ElementOrder;
using Lusa.UI.WorkBenchContract.UI.Controls.Menu;

namespace Lusa.UI.MainPanel.ViewTab
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
