using AddinEngine;

namespace WorkBenchContract
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
