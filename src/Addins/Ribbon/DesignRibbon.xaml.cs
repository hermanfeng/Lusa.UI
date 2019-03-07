using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Infragistics.Windows.Ribbon;
using Lusa.AddinEngine.Extension;
using Lusa.UI.Ribbon.Menu;
using Lusa.UI.WorkBenchContract.Controls.Menu;
using Lusa.UI.WorkBenchContract.Controls.Menu.ExtensionPoint;

namespace Lusa.UI.Ribbon
{
    /// <summary>
    /// EditorTabGroup.xaml 的交互逻辑
    /// </summary>
    public partial class DesignRibbon
    {
        public DesignRibbon()
        {
            InitializeComponent();
            this.Loaded += DesignRibbon_Loaded;
        }

        void DesignRibbon_Loaded(object sender, RoutedEventArgs e)
        {
            LoadMenus();
        }

        private void LoadMenus()
        {
            AddExtendMenuItems();
        }

        private void AddDefaultItems(ICollection<IMenuDescriptorProvider<MenuTabDescriptor>> allTabs, ICollection<IMenuDescriptorProvider<MenuGroupDescriptor>> allGroups)
        {
            allTabs.Add(new OptionsRibbonTabDescriptorProvider());
            allGroups.Add(new OptionsRibbonGroupDescriptorProvider());
        }


        private void AddExtendMenuItems()
        {
            var tabs = new Dictionary<string, RibbonTabItem>();
            var groupItems = new Dictionary<string, ToolHorizontalWrapPanel>();

            var tabBuilder = new MenuTabDescriptorBuilder();
            var allTabProviderItems = tabBuilder.BuildItems().GeneratedItems;

            var groupBuilder = new MenuGroupDescriptorBuilder();
            var allGroupProviders = groupBuilder.BuildItems().GeneratedItems;

            AddDefaultItems(allTabProviderItems, allGroupProviders);

            BuildRibbonTabs(allTabProviderItems, tabs);

            BuildRibbonGroups(allGroupProviders, tabs, groupItems);

            BuildRibbonItems(groupItems);
        }

        private void BuildRibbonItems(IDictionary<string, ToolHorizontalWrapPanel> groupItems)
        {
            var itemBuilder = new MenuItemDescriptorBuilder();
            var items = itemBuilder.BuildItems().GeneratedItems.ReorderElements().Select(itemProvider => itemProvider.Item);
            items.ForEach(item =>
            {
                if (item.Location == MenuItemLocation.TabMenu)
                {
                    var panel = groupItems.GetValue(item.GroupId);
                    if (panel.IsNotNull())
                    {
                        var tool = BuildToolByMenuItemDescriptor(item);
                        if (tool.IsNotNull())
                        {
                            SetupRibbonTool(tool, item);
                            panel.Children.Add(tool);
                        }
                    }
                }
                else if(item.Location == MenuItemLocation.ApplicationMenu)
                {
                    var tool = BuildToolByMenuItemDescriptor(item);
                    if (tool.IsNotNull())
                    {
                        SetupRibbonTool(tool, item);
                        this.applicationMenu.Items.Add(tool);
                    }
                }
                else if(item.Location == MenuItemLocation.AreaToolBar)
                {
                    var tool = BuildToolByMenuItemDescriptor(item);
                    if (tool.IsNotNull())
                    {
                        SetupRibbonTool(tool, item);
                        this.tabItemAreaToolbar.Items.Add(tool);
                    }
                }
            });
        }

        private void SetupRibbonTool(UIElement tool, MenuItemDescriptor item)
        {
            RibbonToolHelper.SetId(tool, item.Id);
            //if (item.ImageUrl.IsNotNullOrEmpty())
            //{
            //    RibbonToolHelper.SetLargeImage(tool, new BitmapImage(new Uri(item.ImageUrl)));
            //}
            if (item.Name.IsNotNull())
            {
                RibbonToolHelper.SetCaption(tool, item.Name);
            }
            if (item.CanQuicklyAccess)
            {
                quickAccessToolbar.Items.Add(new QatPlaceholderTool(item.Id));
            }
            
        }

        private UIElement BuildToolByMenuItemDescriptor(MenuItemDescriptor descriptor)
        {
            if (descriptor.ContentType.IsNotNull())
            {
                if (descriptor.Location == MenuItemLocation.TabMenu && descriptor.ContentType.Is<IRibbonTool>())
                {
                    return descriptor.ContentType.CreateInstance<UIElement>();
                }
                else if(descriptor.Location == MenuItemLocation.AreaToolBar || descriptor.Location == MenuItemLocation.ApplicationMenu)
                {
                    return descriptor.ContentType.CreateInstance<UIElement>();
                }
            }
            return null;
        }

        private static void BuildRibbonGroups(IEnumerable<IMenuDescriptorProvider<MenuGroupDescriptor>> allGroupProviders, IDictionary<string, RibbonTabItem> tabs, Dictionary<string, ToolHorizontalWrapPanel> groupItems)
        {
            var grous = allGroupProviders.ReorderElements().Select(itemProvider => itemProvider.Item);
            grous.ForEach(groupDes =>
            {
                var tabItem = tabs.GetValue(groupDes.TabId);
                if (tabItem.IsNotNull())
                {
                    var groupItem = new RibbonGroup() {Caption = groupDes.GroupName, Id = groupDes.GroupId};
                    var groupPanel = new ToolHorizontalWrapPanel();
                    groupItem.Items.Add(groupPanel);
                    tabItem.RibbonGroups.Add(groupItem);
                    groupItems.Add(groupItem.Id, groupPanel);
                }
            });
        }

        private void BuildRibbonTabs(IEnumerable<IMenuDescriptorProvider<MenuTabDescriptor>> allTabProviderItems, IDictionary<string, RibbonTabItem> tabs)
        {
            var gTabs = allTabProviderItems.ReorderElements().Select(itemProvider => itemProvider.Item);
            gTabs.ForEach(tabDes =>
            {
                var tabitem = new RibbonTabItem() {Header = tabDes.TabName, Uid = tabDes.TabId};
                this.myRibbon.Tabs.Add(tabitem);
                tabs.Add(tabitem.Uid, tabitem);
            });
        }
    }


}
