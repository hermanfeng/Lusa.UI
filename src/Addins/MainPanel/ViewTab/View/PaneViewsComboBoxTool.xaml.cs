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
using Infragistics.Windows.DockManager;
using Infragistics.Windows.Ribbon;
using Infragistics.Windows.Ribbon.Internal;
using WorkBenchContract;
using CommonLibrary;

namespace MainPanelPlugin
{
    /// <summary>
    /// Interaction logic for ComboBoxTool.xaml
    /// </summary>
    public partial class PaneViewsComboBoxTool : UserControl , IRibbonTool
    {

        public PaneViewsComboBoxTool()
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            try
            {
                var dockManager = MainDockPanel.DockManager;
                if (dockManager.IsNotNull())
                {
                    IEnumerable<SplitPane> splitPanes = MainDockPanel.DockManager.Panes;
                    MainDockPanel.DockManager.Content.As<DocumentContentHost>(host =>
                    {
                        splitPanes = host.Panes.Union(splitPanes);
                    });

                    splitPanes.SelectMany(pane => pane.Panes).OfType<TabGroupPane>().SelectMany(groupPane => groupPane.Items.OfType<ContentPane>())
                        .ForEach(contentPane => this.Menu.Items.Add(BuildMenuItem(contentPane)));
                }
            }
            catch(Exception ex)
            {
                MessageService.Instance.SendMessage(ex);
            }
            
        }

        private ToolMenuItem BuildMenuItem(ContentPane contentPane)
        {
            var item = new ToolMenuItem() {Header = contentPane.Header};
            item.Tag = contentPane;
            //item.Icon
            item.Click += item_Click;
            return item;
        }

        void item_Click(object sender, RoutedEventArgs e)
        {
            sender.As<ToolMenuItem>(menuitem =>
            {
                menuitem.Tag.As<ContentPane>(pane =>
                {
                    if (pane.Visibility != Visibility.Visible)
                    {
                        pane.Visibility = Visibility.Visible;
                    }
                    if (pane.IsActivePane)
                    {
                        pane.Activate();
                    }
                });
            });
        }

        Infragistics.Windows.Ribbon.Internal.RibbonToolProxy IRibbonTool.ToolProxy
        {
            get { return new ComboBoxToolRibbonToolProxy(); }
        }

        public class ComboBoxToolRibbonToolProxy : RibbonToolProxy<PaneViewsComboBoxTool>
        {
             
        }
    }

    
}
