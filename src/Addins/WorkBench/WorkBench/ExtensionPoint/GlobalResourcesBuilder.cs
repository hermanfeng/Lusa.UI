using System.Windows;
using UIShell.OSGi;

namespace AddinEngine
{
    public class GlobalResourcesPointBuilder : ClassPointBuilder<ResourceDictionary>
    {
        public const string GlobalResourcesPoint = "Global.Resources";

        public override string ExensionPoint
        {
            get { return GlobalResourcesPoint; }
        }
    }
}
