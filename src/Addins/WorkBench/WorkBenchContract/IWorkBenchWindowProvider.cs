using System.Windows;

namespace Lusa.UI.WorkBenchContract
{
    public interface IWorkBenchWindowProvider
    {
        Window MainWindow { get; }
    }
}
