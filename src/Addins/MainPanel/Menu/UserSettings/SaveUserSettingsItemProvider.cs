using Infragistics.Windows.Ribbon;
using Lusa.AddinEngine.ElementOrder;
using Lusa.UI.WorkBenchContract.Commands;
using Lusa.UI.WorkBenchContract.Controls.Menu;

namespace Lusa.UI.MainPanel.Menu.UserSettings
{
    [ElementOrder(30)]
    public class SaveUserSettingsItemProvider : DefaultMenuItemDescriptorProvider
    {
        public SaveUserSettingsItemProvider()
        {
            this.MenuItem = new MenuItemDescriptor()
            {
                Id = "SaveSettings",
                Name = "Save",
                GroupId = "TView.UserSettings", 
                Location = MenuItemLocation.TabMenu,
                ContentType = typeof(ButtonTool),
                Command = ViewCommands.SaveSettings,
                ImageUrl = "/images/save.png"
            };
        }
    }
}
