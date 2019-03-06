using System.Windows;
using Lusa.AddinEngine.ExtendsionPoint;

namespace Lusa.UI.WorkBench.ExtensionPoint
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
