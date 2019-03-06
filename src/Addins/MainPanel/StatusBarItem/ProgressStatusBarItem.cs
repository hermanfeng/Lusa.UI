namespace Lusa.UI.MainPanel.StatusBarItem
{
    public class ProgressStatusBarItem : WorkBenchContract.ExtensionPoint.StatusBarItem
    {
        public ProgressStatusBarItem()
        {
            this.IsLeft = false;
            this.Content = new ProgressItem();
        }
    }
}
