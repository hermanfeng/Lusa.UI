using Lusa.UI.WorkBenchContract.UI.Controls.Menu;

namespace Lusa.UI.MainPanel.ViewTab.View
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
                //ContentType = typeof(PaneViewsComboBoxTool),
                ImageUrl = "/images/panviews.png"
            };
        }
    }

}
