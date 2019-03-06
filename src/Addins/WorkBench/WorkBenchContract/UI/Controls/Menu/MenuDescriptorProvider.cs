namespace WorkBenchContract
{
    public abstract class MenuDescriptorProvider<T> : IMenuDescriptorProvider<T>
    {

        public abstract T Item { get; }
    }
}
