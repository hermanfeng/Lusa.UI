using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Lusa.UI.WorkBench
{
    public partial class WorkBenchMainWindow : Window
    {
        public WorkBenchMainWindow()
        {
            InitializeComponent();
            this.Icon = new BitmapImage(new Uri("App.ico", UriKind.RelativeOrAbsolute));
            //this.WindowState = System.Windows.WindowState.Maximized;
        }
    }
}
