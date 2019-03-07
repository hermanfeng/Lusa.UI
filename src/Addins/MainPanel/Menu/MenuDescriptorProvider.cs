using Lusa.UI.WorkBenchContract.Controls.Menu;

namespace Lusa.UI.MainPanel.Menu
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
