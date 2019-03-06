namespace AddinEngine
{
    public class ExtensionDataProviderBase : IExtensionDataProvider
    {
        private ExtensionDatas _metadatas;

        public ExtensionDatas ExtensionDatas
        {
            get
            {
                if (_metadatas == null)
                {
                    _metadatas = new ExtensionDatas();
                }
                return _metadatas;
            }
        }
    }
}