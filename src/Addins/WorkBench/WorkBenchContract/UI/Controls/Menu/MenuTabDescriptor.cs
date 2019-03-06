using Lusa.AddinEngine.Extension;
using Lusa.UI.Msic.ViewModelBase;

namespace Lusa.UI.WorkBenchContract.UI.Controls.Menu
{
    public class MenuTabDescriptor : ViewModelBase
    {
        public MenuTabDescriptor(string tabId, string tabName = "")
        {
            if (tabName.IsNullOrEmpty())
            {
                tabName = tabId;
            }
            this.tabId = tabId;
            this.tabName = tabName;
        }

        private string tabName = string.Empty;
        public string TabName
        {
            get { return this.tabName; }
            set
            {
                if (this.tabName != value)
                {
                    this.tabName = value;
                    base.RaisePropertyChanged("TabName");
                }
            }
        }

        private string tabId = string.Empty;
        public string TabId
        {
            get { return this.tabId; }
            set
            {
                if (this.tabId != value)
                {
                    this.tabId = value;
                    base.RaisePropertyChanged("TabId");
                }
            }
        }

        public override bool Equals(object obj)
        {
            return obj.As<MenuTabDescriptor, bool>(ginfo => ginfo.TabId == this.TabId);
        }

        public override int GetHashCode()
        {
            return this.tabId.GetHashCode();
        }
    }


}