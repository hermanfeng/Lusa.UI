namespace Lusa.AddinEngine.ExtendsionPoint
{
    public interface IExtensionInitializer<T> where T : class
    {
        void Initialize(InitializeArgs<T> args);
    }

    public class InitializeArgs<T> where T : class
    {
        ExtensionPointBuilderBase<T> builder;

        public ExtensionPointBuilderBase<T> Builder
        {
            get { return builder; }
            set { builder = value; }
        }

        object tag;

        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        public object TagExt { get; set; }
    }
}
