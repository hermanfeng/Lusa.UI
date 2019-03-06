using System.Windows;

namespace WorkBenchContract
{
    public interface IWorkBenchInitializer
    {
        void Initialize(FrameworkElement topElement);
    }
}
