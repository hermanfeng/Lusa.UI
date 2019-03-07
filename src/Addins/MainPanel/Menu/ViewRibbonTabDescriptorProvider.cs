using Lusa.AddinEngine.ElementOrder;
using Lusa.UI.WorkBenchContract.Controls.Menu;

namespace Lusa.UI.MainPanel.Menu
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
