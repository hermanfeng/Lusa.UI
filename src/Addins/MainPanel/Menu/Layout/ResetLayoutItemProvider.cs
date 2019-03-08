using Infragistics.Windows.Ribbon;
using Lusa.AddinEngine.ElementOrder;
using Lusa.UI.WorkBenchContract.Commands;
using Lusa.UI.WorkBenchContract.Controls.Menu;

namespace Lusa.UI.MainPanel.Menu.Layout
{
    [ElementOrder(30)]
    public class ResetLayoutItemProvider : DefaultMenuItemDescriptorProvider
    {
        public ResetLayoutItemProvider()
        {
            this.MenuItem = new MenuItemDescriptor()
            {
                Id = "ResetLayout",
                Name = "Reset",
                GroupId = "TView.Layout", 
                Location = MenuItemLocation.TabMenu,
                ContentType = typeof(ButtonTool),
                Command = ViewCommands.ResetLayout,
                ImageUrl = "/images/reset.png"
            };
        }
    }
}
