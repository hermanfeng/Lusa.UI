using Lusa.AddinEngine.ExtendsionPoint;
using Lusa.UI.WorkBenchContract.Controls.Pane;

namespace Lusa.UI.WorkBenchContract.ExtensionPoint

{
    public class PaneViewPointBulder : ClassPointBuilder<IPaneViewDescriptorProvider>
    {
        public const string PanViewPoint = "WorkBench.PaneViewPoint";

        public override string ExensionPoint
        {
            get { return PanViewPoint; }
        }
    }
}
