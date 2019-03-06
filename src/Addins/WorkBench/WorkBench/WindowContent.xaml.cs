using System;
using System.ComponentModel;
using Lusa.AddinEngine.Extension;
using Lusa.UI.WorkBenchContract.ExtensionPoint;

namespace Lusa.UI.WorkBench
{
    public partial class EmbedContent : INotifyPropertyChanged
    {
        public EmbedContent()
        {
            this.InitializeComponent();

            this.DataContext = this;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            InitializeUI();
        }

        private void InitializeUI()
        {
            InitializeToolBarPanel();
            InitializeStatusBarPanel();
            InitializeMainPanel();
            CustomInitialize();
        }

        private void InitializeMainPanel()
        {
            var b = new MainPanelContentPointBulder()
                        {Tag = this.avalondockContainer};
            b.BuildItems().GeneratedItems.ForEach( item => this.avalondockContainer.Children.Add(item));
        }

        private void InitializeToolBarPanel()
        {
            var b = new ToolBarContentPointBulder() {Tag = this.ribbonContainer};

        }

        private void InitializeStatusBarPanel()
        {
            var b = new StatusBarContentlPointBulder() {Tag = this.statusbarContainer};
            b.BuildItems().GeneratedItems.ForEach(item => this.statusbarContainer.Children.Add(item.Content));
        }

        private void CustomInitialize()
        {
            var b = new WorkBenchInitializerPointBuilder();
            b.BuildItems().GeneratedItems.ForEach(ini => ini.Initialize(this));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
