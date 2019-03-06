using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Serialization;
using AddinEngine;
using CommonExtension;
using CommonLibrary;

namespace WorkBenchContract
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