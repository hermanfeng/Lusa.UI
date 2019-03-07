using Lusa.AddinEngine.ExtendsionPoint;

namespace Lusa.UI.WorkBenchContract.Controls.Menu.ExtensionPoint
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
