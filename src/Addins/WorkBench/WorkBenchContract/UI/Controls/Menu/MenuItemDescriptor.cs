using System;
using System.Windows.Input;
using Lusa.AddinEngine.Extension;
using Lusa.UI.Msic.ViewModelBase;

namespace Lusa.UI.WorkBenchContract.UI.Controls.Menu
{
    public class MenuItemDescriptor : ViewModelBase
    {
        public static string IconImagePath = "";
        private string _id;
        private string _name;
        private string _value;
        private string _imageUrl; //@"../images/app.ico";
        private ICommand _command;


        public string Id
        {
            get
            {
                if (_id.IsNullOrEmpty())
                {
                    return Name;
                }
                return _id;
            }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string Name
        {
            get
            {
                if (_name.IsNullOrEmpty() && ContentType.IsNotNull())
                {
                    return ContentType.Name;
                }
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    base.RaisePropertyChanged();
                }
            }
        }
        public string ImageUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_imageUrl))
                {
                    return null;
                }
                return _imageUrl;
                //return ImageFilePathProvider.GetImageLocalPath(imageUrl);
            }
            set
            {
                if (_imageUrl != value)
                {
                    _imageUrl = value;
                    RaisePropertyChanged();
                }
            }
        }

        public MenuItemDescriptor()
        {
            GroupId = "Others";
            SizeType = MenuItemSizeType.ImageAndTextLarge;
        }

        public ICommand Command
        {
            get { return _command; }
            set
            {
                if (_command != value)
                {
                    _command = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string GroupId { get; set; }

        public bool CanQuicklyAccess { get; set; }

        public MenuItemLocation Location { get; set; }

        public Type ContentType { get; set; }

        public MenuItemSizeType SizeType { get; set; }

    }


    public enum MenuItemSizeType
    {
		ImageOnly,
		ImageAndTextNormal,
		ImageAndTextLarge
    }

    public enum MenuItemLocation
    {
        ApplicationMenu,
        TabMenu,
        AreaToolBar
    }

}