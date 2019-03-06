using Lusa.AddinEngine.ElementOrder;
using Lusa.UI.WorkBenchContract.UI.Controls.Menu;

namespace Lusa.UI.MainPanel.ViewTab
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
