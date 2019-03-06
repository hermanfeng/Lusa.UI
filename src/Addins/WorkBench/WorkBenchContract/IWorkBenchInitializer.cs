using System.Windows;

namespace Lusa.UI.WorkBenchContract
{
    public interface IWorkBenchInitializer
    {
        void Initialize(FrameworkElement topElement);
    }
}
