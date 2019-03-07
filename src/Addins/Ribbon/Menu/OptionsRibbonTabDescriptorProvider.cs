using Lusa.AddinEngine.ElementOrder;
using Lusa.UI.WorkBenchContract.Controls.Menu;

namespace Lusa.UI.Ribbon.Menu
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
