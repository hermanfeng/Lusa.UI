using Lusa.AddinEngine.Extension;
using Lusa.UI.Msic.ViewModelBase;

namespace Lusa.UI.WorkBenchContract.Controls.Menu
{
    public class MenuGroupDescriptor : ViewModelBase
    {
        public MenuGroupDescriptor(string groupId, string tabid,string groupName="")
        {
            if (groupName.IsNullOrEmpty())
            {
                groupName = groupId;
            }
            this.groupId = groupId;
            this.groupName = groupName;
            this.tabId = tabid;
        }

        private string groupName = string.Empty;
        public string GroupName
        {
            get { return this.groupName; }
            set
            {
                if (this.groupName != value)
                {
                    this.groupName = value;
                    base.RaisePropertyChanged("GroupName");
                }
            }
        }

        private string groupId = string.Empty;
        public string GroupId
        {
            get { return this.groupId; }
            set
            {
                if (this.groupId != value)
                {
                    this.groupId = value;
                    base.RaisePropertyChanged("GroupId");
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
            return obj.As<MenuGroupDescriptor, bool>(ginfo => ginfo.groupId == this.groupId);
        }

        public override int GetHashCode()
        {
            return this.groupId.GetHashCode();
        }
    }

}