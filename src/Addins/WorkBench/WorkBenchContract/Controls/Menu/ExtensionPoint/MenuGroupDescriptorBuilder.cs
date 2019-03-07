using Lusa.AddinEngine.ExtendsionPoint;

namespace Lusa.UI.WorkBenchContract.Controls.Menu.ExtensionPoint
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
