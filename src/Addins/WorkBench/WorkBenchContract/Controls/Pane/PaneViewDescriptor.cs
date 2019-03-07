using System;
using System.Xml.Serialization;
using Lusa.AddinEngine.Extension;
using Lusa.AddinEngine.ExtensionData;

namespace Lusa.UI.WorkBenchContract.Controls.Pane
{
    public class PaneViewDescriptor : ExtensionDatas
    {
        private string _header;
        private string _id;
        private string _name;
        private bool _isActive;
        private bool _isDocument = false;

        public PaneViewDescriptor(object content)
        {
            Content = content;
        }

        public PaneViewDescriptor(Type contentType)
        {
            this.ContentType = contentType;
        }

        public string Id
        {
            get
            {
                if (_id.IsNullOrEmpty())
                {
                    return this.Header;
                }
                return this._id;
            }
            set
            {
                if (this._id != value)
                {
                    this._id = value;
                }
            }
        }

        //FrameWorkElement.Name
        public string Name
        {
            get
            {
                if (_name.IsNullOrEmpty())
                {
                    return this.Header.Replace(" ", "");
                }
                return this._name;
            }
            set
            {
                if (this._name != value)
                {
                    this._name = value;
                }
            }
        }

        public string Header
        {
            get
            {
                return this._header;
            }
            set
            {
                if (this._header != value)
                {
                    this._header = value;
                }
            }
        }

        public bool IsActive
        {
            get
            {
                return this._isActive;
            }
            set
            {
                if (this._isActive != value)
                {
                    this._isActive = value;
                }
            }
        }

        public bool IsPinned { get; set; }

        public string ImageUrl { get; set; }

        [XmlAttribute]
        public bool IsDocument
        {
            get
            {
                return this._isDocument;
            }
            set
            {
                if (this._isDocument != value)
                {
                    this._isDocument = value;
                }
            }
        }

        public DockLocation Location { get; set; }
        public Type ContentType
        {
            get;
            private set;
        }

        public object Content { get; private set; }
    }

    public enum DockLocation
    {
        Left,
        Right,
        Bottom
    }

    public interface IUIContentRequest
    {
        object Content { set; }
    }
}