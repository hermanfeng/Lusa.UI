﻿using Infragistics.Windows.Ribbon;
using Lusa.AddinEngine.ElementOrder;
using Lusa.UI.WorkBenchContract.Commands;
using Lusa.UI.WorkBenchContract.Controls.Menu;

namespace Lusa.UI.MainPanel.Menu.Layout
{
    [ElementOrder(10)]
    public class SaveLayoutItemProvider : DefaultMenuItemDescriptorProvider
    {
        public SaveLayoutItemProvider()
        {
            this.MenuItem = new MenuItemDescriptor()
            {
                Id = "SaveLayout",
                Name = "Save",
                GroupId = "TView.Layout", 
                Location = MenuItemLocation.TabMenu,
                ContentType = typeof(ButtonTool),
                ImageUrl = "/images/save.png",
                Command = ViewCommands.SaveLayout,
            };
        }
    }
}
