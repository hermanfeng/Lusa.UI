using Lusa.UI.WorkBenchContract.Controls.Pane;

namespace TestPaneView
{
    public class TestPaneViewPanViewProvider : IPaneViewDescriptorProvider
    {
        PaneViewDescriptor IPaneViewDescriptorProvider.Pane
        {
            get
            {
                return new PaneViewDescriptor(typeof(PaneViewUserControl)) { Header = "TestPaneView", IsDocument = true };
            }
        }
    }
}
