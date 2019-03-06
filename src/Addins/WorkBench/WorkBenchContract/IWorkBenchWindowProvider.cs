using System.Windows;

namespace WorkBenchContract
{
    public interface IWorkBenchWindowProvider
    {
        Window MainWindow { get; }
    }
}
