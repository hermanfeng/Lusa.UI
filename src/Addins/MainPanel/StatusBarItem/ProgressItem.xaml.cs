using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CommonExtension;
using CommonLibrary;

namespace MainPanelPlugin
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
