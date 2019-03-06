using Lusa.UI.WorkBenchContract.UI.Controls.Menu;

namespace Lusa.UI.MainPanel.ViewTab
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
