using AddinEngine;

namespace WorkBenchContract
{
    public class MenuGroupDescriptorBuilder : ClassPointBuilder<IMenuDescriptorProvider<MenuGroupDescriptor>>
    {
        public const string MenuGroupDescriptor = "Menu.GroupDescriptorProvider";

        public override string ExensionPoint
        {
            get { return MenuGroupDescriptor; }
        }
    }


}
