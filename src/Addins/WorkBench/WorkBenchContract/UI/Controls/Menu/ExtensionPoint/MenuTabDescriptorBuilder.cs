using AddinEngine;

namespace WorkBenchContract
{
    public class MenuTabDescriptorBuilder : ClassPointBuilder<IMenuDescriptorProvider<MenuTabDescriptor>>
    {
        public const string MenuTabDescriptor = "Menu.TabDescriptorProvider";

        public override string ExensionPoint
        {
            get { return MenuTabDescriptor; }
        }
    }


}
