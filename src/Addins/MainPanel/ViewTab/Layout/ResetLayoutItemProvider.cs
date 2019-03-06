using AddinEngine;
using WorkBenchContract;

namespace MainPanelPlugin
{
    [ElementOrder(30)]
    public class ResetLayoutItemProvider : DefaultMenuItemDescriptorProvider
    {
        public ResetLayoutItemProvider()
        {
            this.MenuItem = new MenuItemDescriptor()
            {
                Name = "Reset",
                GroupId = "TView.Layout", 
                Location = MenuItemLocation.TabMenu,
                //ContentType = typeof(ButtonTool),
                Command = ViewCommands.ResetLayout,
                ImageUrl = "/images/layout/reset.png"
            };
        }
    }
}
