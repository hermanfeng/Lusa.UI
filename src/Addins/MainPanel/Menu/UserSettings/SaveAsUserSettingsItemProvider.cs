using Infragistics.Windows.Ribbon;
using Lusa.AddinEngine.ElementOrder;
using Lusa.UI.WorkBenchContract.Commands;
using Lusa.UI.WorkBenchContract.Controls.Menu;

namespace Lusa.UI.MainPanel.Menu.UserSettings
{
    [ElementOrder(20)]
    public class SaveAsUserSettingsItemProvider : DefaultMenuItemDescriptorProvider
    {
        public SaveAsUserSettingsItemProvider()
        {
            this.MenuItem = new MenuItemDescriptor()
            {
                Id = "SaveAsSettings",
                Name = "SaveAs",
                GroupId = "TView.UserSettings", 
                Location = MenuItemLocation.TabMenu,
                ContentType = typeof(ButtonTool),
                Command = ViewCommands.SaveAsSettings,
                ImageUrl = "/images/saveas.png"
            };
        }
    }
}
