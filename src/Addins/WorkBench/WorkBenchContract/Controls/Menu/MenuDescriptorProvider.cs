namespace Lusa.UI.WorkBenchContract.Controls.Menu
{
    public abstract class MenuDescriptorProvider<T> : IMenuDescriptorProvider<T>
    {

        public abstract T Item { get; }
    }
}
