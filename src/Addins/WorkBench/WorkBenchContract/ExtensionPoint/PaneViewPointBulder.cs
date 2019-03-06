using AddinEngine;

namespace WorkBenchContract

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
