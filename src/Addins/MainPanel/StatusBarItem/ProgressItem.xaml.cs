using System;
using System.Windows.Controls;
using System.Windows.Data;
using Lusa.AddinEngine.Extension;
using Lusa.UI.Msic.MessageService;
using Lusa.UI.Msic.MessageService.MessageObject;

namespace Lusa.UI.MainPanel.StatusBarItem
{
    /// <summary>
    /// Interaction logic for ProgressItem.xaml
    /// </summary>
    public partial class ProgressItem : UserControl,IMessageListener
    {
        public ProgressItem()
        {
            InitializeComponent();
            MessageService.Instance.RegisterMessageListener(this);
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }

        bool IMessageListener.CanProcess(MessageObject msg)
        {
            return msg.Type == MessageType.Progress;
        }

        void IMessageListener.NotifyMessge(MessageObject msg)
        {
            this.StatusBusyIndicator.Dispatcher.BeginInvoke(new Action(() => msg.As<ProgressMessageObject>(msgObj =>
            {
                if (msgObj.Progress > 0)
                {
                    this.StatusBusyIndicator.IsBusy = true;
                }
                this.StatusBusyIndicator.ProgressValue = msgObj.Progress / 100.0;
            })));

        }
    }

    public class DoubleToPercentConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var p = (double)value;
            return p*100 + "%";
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
 
}
