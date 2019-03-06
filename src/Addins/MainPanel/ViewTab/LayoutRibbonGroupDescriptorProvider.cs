using Lusa.AddinEngine.ElementOrder;
using Lusa.UI.WorkBenchContract.UI.Controls.Menu;

namespace Lusa.UI.MainPanel.ViewTab
{
    [ElementOrder(200)]
    public class LayoutRibbonGroupDescriptorProvider : MenuDescriptorProvider<MenuGroupDescriptor>
    {
        public override MenuGroupDescriptor Item
        {
            get { return new MenuGroupDescriptor("TView.Layout", "TView", "Layout"); }
        }
    }
}
