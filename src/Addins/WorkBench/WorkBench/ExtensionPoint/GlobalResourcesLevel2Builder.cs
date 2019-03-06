using System.Windows;
using UIShell.OSGi;

namespace AddinEngine
{
    public class GlobalResourcesLevel2PointBuilder : ClassPointBuilder<ResourceDictionary>
    {
        public const string GlobalResourcesPoint = "Global.Resources.Level2";

        public override string ExensionPoint
        {
            get { return GlobalResourcesPoint; }
        }
    }
}
