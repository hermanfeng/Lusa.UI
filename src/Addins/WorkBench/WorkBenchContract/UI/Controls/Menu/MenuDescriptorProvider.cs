namespace Lusa.UI.WorkBenchContract.UI.Controls.Menu
{
    public abstract class MenuDescriptorProvider<T> : IMenuDescriptorProvider<T>
    {

        public abstract T Item { get; }
    }
}
