using Infragistics.Windows.Ribbon;
using Lusa.AddinEngine.ElementOrder;
using Lusa.UI.WorkBenchContract.Commands;
using Lusa.UI.WorkBenchContract.Controls.Menu;

namespace Lusa.UI.MainPanel.Menu.Layout
{
    [ElementOrder(20)]
    public class LoadLayoutItemProvider : DefaultMenuItemDescriptorProvider
    {
        public LoadLayoutItemProvider()
        {
            this.MenuItem = new MenuItemDescriptor()
            {
                Name = "Load",
                GroupId = "TView.Layout", 
                Location = MenuItemLocation.TabMenu,
                ContentType = typeof(ButtonTool),
                Command = ViewCommands.LoadLayout,
                ImageUrl = "/images/layout/load.png"
            };
        }
    }
}
