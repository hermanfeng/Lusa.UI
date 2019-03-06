using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using UIShell.OSGi;
using System.Windows.Data;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;

namespace AddinEngine
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
