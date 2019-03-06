using Lusa.AddinEngine.ExtendsionPoint;

namespace Lusa.UI.WorkBenchContract.UI.Controls.Menu.ExtensionPoint
{
    public class MenuItemDescriptorBuilder : ClassPointBuilder<IMenuDescriptorProvider<MenuItemDescriptor>>
    {
        public const string MenuItemDescriptor = "Menu.ItemDescriptorProvider";

        public override string ExensionPoint
        {
            get { return MenuItemDescriptor; }
        }
    }
}
