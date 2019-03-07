using Lusa.AddinEngine.ElementOrder;
using Lusa.UI.WorkBenchContract.UI.Controls.Menu;

namespace LusaRibbon
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
