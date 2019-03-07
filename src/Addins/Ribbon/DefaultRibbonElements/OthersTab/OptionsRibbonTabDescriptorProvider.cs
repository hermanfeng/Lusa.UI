using Lusa.AddinEngine.ElementOrder;
using Lusa.UI.WorkBenchContract.UI.Controls.Menu;

namespace LusaRibbon
{
    [ElementOrder(1000)]
    public class OptionsRibbonTabDescriptorProvider : MenuDescriptorProvider<MenuTabDescriptor>
    {
        public override MenuTabDescriptor Item
        {
            get { return new MenuTabDescriptor("Options", "Options"); }
        }
    }
}
