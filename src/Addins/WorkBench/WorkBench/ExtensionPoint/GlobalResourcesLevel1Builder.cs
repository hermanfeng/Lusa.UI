using System.Windows;
using UIShell.OSGi;

namespace AddinEngine
{
    public class GlobalResourcesLevel1PointBuilder : ClassPointBuilder<ResourceDictionary>
    {
        public const string GlobalResourcesPoint = "Global.Resources.Level1";

        public override string ExensionPoint
        {
            get { return GlobalResourcesPoint; }
        }
    }
}
