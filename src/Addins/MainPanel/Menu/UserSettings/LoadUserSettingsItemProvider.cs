using Infragistics.Windows.Ribbon;
using Lusa.AddinEngine.ElementOrder;
using Lusa.UI.WorkBenchContract.Commands;
using Lusa.UI.WorkBenchContract.Controls.Menu;

namespace Lusa.UI.MainPanel.Menu.UserSettings
{
    [ElementOrder(10)]
    public class LoadUserSettingsItemProvider : DefaultMenuItemDescriptorProvider
    {
        public LoadUserSettingsItemProvider()
        {
            this.MenuItem = new MenuItemDescriptor()
            {
                Id = "LoadSettings",
                Name = "Load",
                GroupId = "TView.UserSettings", 
                Location = MenuItemLocation.TabMenu,
                ContentType = typeof(ButtonTool),
                Command = ViewCommands.LoadSetings,
                ImageUrl = "/images/openfile.png"
            };
        }
    }
}
