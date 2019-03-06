using AddinEngine;
using WorkBenchContract;

namespace MainPanelPlugin
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
