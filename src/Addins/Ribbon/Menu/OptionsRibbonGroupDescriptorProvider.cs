using Lusa.AddinEngine.ElementOrder;
using Lusa.UI.WorkBenchContract.Controls.Menu;

namespace Lusa.UI.Ribbon.Menu
{
    [ElementOrder(1000)]
    public class OptionsRibbonGroupDescriptorProvider : MenuDescriptorProvider<MenuGroupDescriptor>
    {
        public override MenuGroupDescriptor Item
        {
            get { return new MenuGroupDescriptor("Options", "Options"); }
        }
    }
}
