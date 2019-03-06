using System;
using System.Xml;
using UIShell.OSGi;

namespace Lusa.AddinEngine.ExtendsionPoint
{
    public abstract class ClassPointBuilder<T> : ExtensionPointBuilderBase<T> where T : class
    {
        public ClassPointBuilder(IBundle bundle)
            : base(bundle)
        {

        }

        public ClassPointBuilder()
        {
        }

        protected override bool IsValidNode(XmlNode node)
        {
            return node.Name.Equals("instance", StringComparison.InvariantCultureIgnoreCase);
        }

        protected override T CreateInstance(XmlNode subnode, UIShell.OSGi.Extension extension)
        {
            if (subnode.Attributes["class"] != null)
            {
                return (T)Activator.CreateInstance(extension.Owner.LoadClass(subnode.Attributes["class"].Value));
            }
            return default(T);
        }
    }
}
