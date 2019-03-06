using AddinEngine;
using WorkBenchContract;

namespace MainPanelPlugin
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
                //ContentType = typeof(ButtonTool),
                Command = ViewCommands.LoadLayout,
                ImageUrl = "/images/layout/load.png"
            };
        }
    }
}
