using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Infragistics.Windows.DockManager;
using Infragistics.Windows.Ribbon;
using Infragistics.Windows.Ribbon.Internal;
using Lusa.AddinEngine.Extension;
using Lusa.UI.MainPanel.Dock;
using Lusa.UI.Msic.MessageService;

namespace Lusa.UI.MainPanel.ViewTab.View
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
                var dockManager = MainDockPanel.XamDockManager;
                if (dockManager.IsNotNull())
                {
                    IEnumerable<SplitPane> splitPanes = MainDockPanel.XamDockManager.Panes;
                    MainDockPanel.XamDockManager.Content.As<DocumentContentHost>(host =>
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
