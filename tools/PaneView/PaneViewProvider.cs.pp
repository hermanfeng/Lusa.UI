using Lusa.UI.WorkBenchContract.UI.Controls.Pane;

namespace $rootnamespace$
{
    public class $AssemblyName$PanViewProvider : IPaneViewDescriptorProvider
    {
        PaneViewDescriptor IPaneViewDescriptorProvider.Pane
        {
            get
            {
                return new PaneViewDescriptor(typeof(PaneViewUserControl)) { Header = "$AssemblyName$", IsDocument = true };
            }
        }
    }
}
