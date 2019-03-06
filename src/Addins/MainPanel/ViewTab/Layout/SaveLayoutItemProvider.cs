using Lusa.AddinEngine.ElementOrder;
using Lusa.UI.WorkBenchContract.UI.Commands;
using Lusa.UI.WorkBenchContract.UI.Controls.Menu;

namespace Lusa.UI.MainPanel.ViewTab.Layout
{
    [ElementOrder(10)]
    public class SaveLayoutItemProvider : DefaultMenuItemDescriptorProvider
    {
        public SaveLayoutItemProvider()
        {
            this.MenuItem = new MenuItemDescriptor()
            {
                Name = "Save",
                GroupId = "TView.Layout", 
                Location = MenuItemLocation.TabMenu,
                //ContentType = typeof(ButtonTool),
                ImageUrl = "/images/layout/save.png",
                Command = ViewCommands.SaveLayout,
            };
        }
    }
}
