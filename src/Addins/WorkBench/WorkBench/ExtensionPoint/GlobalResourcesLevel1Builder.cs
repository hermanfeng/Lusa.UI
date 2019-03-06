using System.Windows;
using Lusa.AddinEngine.ExtendsionPoint;

namespace Lusa.UI.WorkBench.ExtensionPoint
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
