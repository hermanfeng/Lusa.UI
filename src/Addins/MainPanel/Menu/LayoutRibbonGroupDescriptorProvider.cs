using Lusa.AddinEngine.ElementOrder;
using Lusa.UI.WorkBenchContract.Controls.Menu;

namespace Lusa.UI.MainPanel.Menu
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
