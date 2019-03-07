using Lusa.UI.WorkBenchContract.Controls.Menu;

namespace Lusa.UI.MainPanel.Menu.View
{
    public class PanViewsItemProvider : DefaultMenuItemDescriptorProvider
    {
        public PanViewsItemProvider()
        {
            this.MenuItem = new MenuItemDescriptor()
            {
                Name = "PanViews",
                GroupId = "TView.PaneView", 
                Location = MenuItemLocation.TabMenu,
                ContentType = typeof(PaneViewsComboBoxTool),
                ImageUrl = "/images/panviews.png"
            };
        }
    }

}
