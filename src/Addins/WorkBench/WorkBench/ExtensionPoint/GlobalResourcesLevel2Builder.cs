using System.Windows;
using Lusa.AddinEngine.ExtendsionPoint;

namespace Lusa.UI.WorkBench.ExtensionPoint
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
