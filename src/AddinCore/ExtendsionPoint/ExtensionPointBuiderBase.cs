using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Lusa.AddinEngine.ElementOrder;
using UIShell.OSGi;
using UIShell.OSGi.Core.Service;

namespace Lusa.AddinEngine.ExtendsionPoint
{
    public abstract class ExtensionPointBuilderBase<T> where T : class
    {
        IBundle bundle;
        public ExtensionPointBuilderBase(IBundle bundle)
        {
            this.bundle = bundle;
            
        }

        public ExtensionPointBuilderBase()
        {
        }

        private IEnumerable<UIShell.OSGi.Extension> GetExtendsions()
        {
            if (Bundle != null && bundle.Context != null)
            {
                return Bundle.Context.GetExtensions(ExensionPoint);
            }
            else
            {
                var manager = BundleRuntime.Instance.GetFirstOrDefaultService<IExtensionManager>();
                if (manager != null)
                {
                    return manager.GetExtensions(ExensionPoint);
                }
            }
            return new List<UIShell.OSGi.Extension>();
        }

        public IBundle Bundle
        {
            get
            {
                return bundle;
            }
        }

        public abstract string ExensionPoint { get; }

        public BuildItemCollection<T> BuildItems() 
        {
            var list = new BuildItemCollection<T>(this);
            var exts = GetExtendsions();
            foreach (UIShell.OSGi.Extension ext in exts)
            {
                foreach (XmlNode dataNode in ext.Data)
                {
                    if (dataNode is XmlComment)
                    {
                        continue;
                    }

                    if (IsValidNode(dataNode))
                    {
                        T t = BuildItem(dataNode, ext);
                        if (t != null)
                        {
                            list.Add(t,ext);
                        }
                    }
                }
            }
            return list;
        }

        protected T BuildItem(XmlNode node, UIShell.OSGi.Extension extension)
        {
            T instance = CreateInstance(node, extension);
            return instance;
        }


        public void Initialize(T instance, UIShell.OSGi.Extension extension)
        {
            InitializeCore(instance, extension);
        }
        protected virtual void InitializeCore(T instance, UIShell.OSGi.Extension extension)
        {
            
        }

        object tag;

        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        object tagext;
        public object TagExt
        {
            get { return tagext; }
            set { tagext = value; }
        }



        protected virtual bool IsValidNode(XmlNode node)
        {
            return node.Name.Equals(KeyWordType.Name, StringComparison.InvariantCultureIgnoreCase);
        }

        protected virtual T CreateInstance(XmlNode subnode, UIShell.OSGi.Extension extension)
        {
            try
            {
                T instance = Activator.CreateInstance<T>();
                return instance;
            }
            catch
            {
                return default(T);
            }
        }

        public Type KeyWordType { get { return typeof(T); } }

    }

    public class BuildItemCollection<T>:Dictionary<T,UIShell.OSGi.Extension> where T : class
    {
        ExtensionPointBuilderBase<T> builder;
        public BuildItemCollection(ExtensionPointBuilderBase<T> builder)
        {
            this.builder = builder;
        }

        bool initialized = false;
        public void Initialize()
        {
            if (!initialized)
            {
                initialized = true;
                foreach(T instance in this.Keys)
                {
                    UIShell.OSGi.Extension extension = this[instance];
                    InitializeCore(instance, extension);
                    builder.Initialize(instance, extension);
                }
            }
        }

        protected virtual InitializeArgs<T> CreateInitializeArgs(T instance)
        {
            var arg = new InitializeArgs<T>();
            arg.Builder = builder;
            arg.Tag = builder.Tag;
            arg.TagExt = builder.TagExt;
            return arg;
        }

        protected virtual void InitializeCore(T instance, UIShell.OSGi.Extension extension)
        {
            var ini = instance as IExtensionInitializer<T>;
            if (ini != null && builder!=null)
            {
                ini.Initialize(CreateInitializeArgs(instance));
            }
        }

        public ICollection<T> GeneratedItems
        {
            get
            {
                if(!initialized)
                {
                    Initialize();
                }

                //reorder
                var reorder = new ElementReorder<T>();
                return reorder.Reorder(this.Keys).ToList();
            }
        }

    }
}
