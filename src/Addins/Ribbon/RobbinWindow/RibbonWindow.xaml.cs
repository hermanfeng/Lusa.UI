using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Infragistics.Controls.Interactions;
using Infragistics.Windows.Ribbon;
using Lusa.AddinEngine.Extension;
using Lusa.UI.Msic.ConfigrationService;
using Lusa.UI.Msic.MessageService;
using Lusa.UI.Msic.MessageService.MessageObject;
using Lusa.UI.WorkBenchContract.ExtensionPoint;
using Lusa.UI.WorkBenchContract.UI;
using Lusa.UI.WorkBenchContract.UI.Controls.Menu;
using Lusa.UI.WorkBenchContract.UI.Controls.Menu.ExtensionPoint;
using Lusa.UI.WorkBenchContract.UI.Controls.Pane;

namespace LusaRibbon
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class RibbonWindow : IMessageListener
    {
        string oldtitle = string.Empty;
        public RibbonWindow()
        {
            InitializeComponent();
            MessageService.Instance.RegisterMessageListener(this);
            oldtitle = this.Title;
            Instance_ConfigChanged(null, null);
            ConfigurationService.Instance.ConfigChanged += Instance_ConfigChanged;
        }

        void Instance_ConfigChanged(object sender, EventArgs e)
        {
            if (ConfigurationService.Instance.CurrentConfigName.IsNotNullOrEmpty())
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.Title = oldtitle + " - " + ConfigurationService.Instance.CurrentConfigName;
                    }));
            }
        }


        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            InitializeUI();
            this.myRibbon.Loaded += myRibbon_Loaded;
        }

        void myRibbon_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(this.myRibbon);
            CommandManager.InvalidateRequerySuggested();
        }

        private void InitializeUI()
        {
            InitializeStatusBarPanel();
            InitializeMainPanel();
            CustomInitialize();
            LoadMenus();
        }

        private void InitializeMainPanel()
        {
            var buider = new MainPanelContentPointBulder();
            var allContentItems = buider.BuildItems().GeneratedItems;
            if (allContentItems.Count > 1)
            {
               allContentItems.ForEach(item => this.mainPanel.Children.Add(item));
            }
            else if(allContentItems.Count == 1)
            {
                var item = allContentItems.ElementAt(0);
                item.HorizontalAlignment = HorizontalAlignment.Stretch;
                item.VerticalAlignment = VerticalAlignment.Stretch;
                this.mainPanel.Children.Add(item);
            }  
        }

        private void InitializeStatusBarPanel()
        {
            var b = new StatusBarContentlPointBulder();
            var allStatusItems = b.BuildItems().GeneratedItems;
            allStatusItems.Where(item => item.IsLeft && item.Content!=null).ForEach(item => this.statusLeftBar.Children.Add(item.Content));
            allStatusItems.Where(item => !item.IsLeft && item.Content != null).ForEach(item => this.statusRightBar.Children.Add(item.Content));
        }

        private void CustomInitialize()
        {
            var b = new WorkBenchInitializerPointBuilder();
            b.BuildItems().GeneratedItems.ForEach(ini => ini.Initialize(this));
        }

        private void LoadMenus()
        {
            AddExtendMenuItems();
        }

        private void AddDefaultItems(ICollection<IMenuDescriptorProvider<MenuTabDescriptor>> allTabs, ICollection<IMenuDescriptorProvider<MenuGroupDescriptor>> allGroups)
        {
            allTabs.AddRange(typeof (RibbonWindow).GetAssemblyTypeInstances<MenuDescriptorProvider<MenuTabDescriptor>>());
            allGroups.AddRange(typeof(RibbonWindow).GetAssemblyTypeInstances<MenuDescriptorProvider<MenuGroupDescriptor>>());
        }

        private void AddDefaultItems(ICollection<IMenuDescriptorProvider<MenuItemDescriptor>> des)
        {
            des.AddRange(typeof(RibbonWindow).GetAssemblyTypeInstances<IMenuDescriptorProvider<MenuItemDescriptor>>());
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
            var itemProviders = itemBuilder.BuildItems().GeneratedItems;
            AddDefaultItems(itemProviders);
            var items =
                itemProviders.ReorderElements()
                    .Select(itemProvider => new {Item = itemProvider.Item, Provider = itemProvider});
            items.ForEach(tupleItem =>
            {
                var item = tupleItem.Item;
                if (item.Location == MenuItemLocation.TabMenu)
                {
                    var panel = groupItems.GetValue(item.GroupId);
                    if (panel.IsNotNull())
                    {
                        var tool = BuildToolByMenuItemDescriptor(item);
                        if (tool.IsNotNull())
                        {
                            SetupRibbonTool(tool, item, tupleItem.Provider);
                            panel.Children.Add(tool);
                        }
                    }
                    else
                    {
                        MessageService.Instance.SendMessage("No group named "+item.GroupId,MessageType.ERROR);
                    }
                }
                else if (item.Location == MenuItemLocation.ApplicationMenu)
                {
                    var tool = BuildToolByMenuItemDescriptor(item);
                    if (tool.IsNotNull())
                    {
                        SetupRibbonTool(tool, item, tupleItem.Provider);
                        this.applicationMenu.Items.Add(tool);
                    }
                }
                else if (item.Location == MenuItemLocation.AreaToolBar)
                {
                    var tool = BuildToolByMenuItemDescriptor(item);
                    if (tool.IsNotNull())
                    {
                        SetupRibbonTool(tool, item, tupleItem.Provider);
                        this.tabItemAreaToolbar.Items.Add(tool);
                    }
                }
            });
        }

        private void SetupRibbonTool(UIElement tool, MenuItemDescriptor item, IMenuDescriptorProvider<MenuItemDescriptor> itemProvider)
        {
            RibbonToolHelper.SetId(tool, item.Id);
            if (item.ImageUrl.IsNotNullOrEmpty())
            {
                try
                {
                    var path = ImageFilePathProvider.GetAssemblyLocalPath(itemProvider.GetType().Assembly, item.ImageUrl);
                    RibbonToolHelper.SetLargeImage(tool, new BitmapImage(new Uri(path)));
                }
                catch (Exception ex)
                {
                    MessageService.Instance.SendMessage(@"Not valid Image Url " + "\" " + item.ImageUrl + " \"", MessageType.ERROR);
                    MessageService.Instance.SendMessage(ex);
                }
                
            }

            tool.As<ButtonBase>(button => button.Command = item.Command);
            tool.As<MenuTool>(menuTool => menuTool.Command = item.Command);

            if (item.Name.IsNotNull())
            {
                RibbonToolHelper.SetCaption(tool, item.Name);
            }
            if (item.CanQuicklyAccess)
            {
                quickAccessToolbar.Items.Add(new QatPlaceholderTool(item.Id));
            }

            if (item.SizeType == MenuItemSizeType.ImageAndTextLarge)
            {
                RibbonGroup.SetMaximumSize(tool, RibbonToolSizingMode.ImageAndTextLarge);
            }
            else if (item.SizeType == MenuItemSizeType.ImageAndTextNormal)
            {
                RibbonGroup.SetMaximumSize(tool, RibbonToolSizingMode.ImageAndTextLarge);
            }
            else
            {
                RibbonGroup.SetMaximumSize(tool, RibbonToolSizingMode.ImageOnly);
            }

            itemProvider.As<IUIContentRequest>(resquest => resquest.Content = tool);
        }

        private UIElement BuildToolByMenuItemDescriptor(MenuItemDescriptor descriptor)
        {
            if (descriptor.ContentType.IsNotNull())
            {
                if (descriptor.Location == MenuItemLocation.TabMenu && descriptor.ContentType.Is<IRibbonTool>())
                {
                    return descriptor.ContentType.CreateInstance<UIElement>();
                }
                else if (descriptor.Location == MenuItemLocation.AreaToolBar || descriptor.Location == MenuItemLocation.ApplicationMenu)
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
                    var groupItem = new RibbonGroup() { Caption = groupDes.GroupName, Id = groupDes.GroupId };
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
                var tabitem = new RibbonTabItem() { Header = tabDes.TabName, Uid = tabDes.TabId };
                this.myRibbon.Tabs.Add(tabitem);
                tabs.Add(tabitem.Uid, tabitem);
            });
        }

        #region Progress Messgae
        bool IMessageListener.CanProcess(MessageObject msg)
        {
            return msg.NeedPopup || msg.As<ProgressMessageObject, bool>(msgObject => msgObject.IsGlobalProgess);
        }

        object currentSender = null;
        Dictionary<object, List<ProgressMessageObject>> allNotFinishedProgressMessages = new Dictionary<object, List<ProgressMessageObject>>();
        void IMessageListener.NotifyMessge(MessageObject msg)
        {
            if (msg.NeedPopup)
            {
                PopupDialogWindow(msg);
            }
            else
            {
                msg.As<ProgressMessageObject>(msgObject => SendProgressMessage(msgObject));
            }
        }

        private XamDialogWindow pupopDialogWindow;
        private void PopupDialogWindow(MessageObject msg)
        {
            if (pupopDialogWindow == null)
            {
                var window = new XamDialogWindow();
                //window.ModalBackgroundEffect = new BlurEffect() {Radius = 10};
                window.IsModal = true;
                window.StartupPosition = StartupPosition.Center;
                window.MinWidth = 400;
                window.MinHeight = 200;

                window.HorizontalContentAlignment = HorizontalAlignment.Center;
                window.VerticalContentAlignment = VerticalAlignment.Center;
                pupopDialogWindow = window;
                var txtBlock = new TextBlock();
                txtBlock.TextWrapping = TextWrapping.WrapWithOverflow;
                txtBlock.FontSize = 18;
                txtBlock.HorizontalAlignment = HorizontalAlignment.Center;
                txtBlock.VerticalAlignment = VerticalAlignment.Center;
                pupopDialogWindow.Content = txtBlock;
                this.mainPanel.Children.Add(pupopDialogWindow);
            }

            ((TextBlock)pupopDialogWindow.Content).Text = msg.Message;
            pupopDialogWindow.Header = msg.Type.ToString();
            pupopDialogWindow.Show();
        }

        private void SendProgressMessage(ProgressMessageObject msgObject)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (currentSender.IsNull() || currentSender == msgObject.Sender)
                    {
                        currentSender = msgObject.Sender;
                        this.GlobalBusyIndicator.IsBusy = !msgObject.IsFinished;
                        this.GlobalBusyIndicator.IsIndeterminate = msgObject.IsIndeterminate;
                        this.GlobalBusyIndicator.ProgressValue = msgObject.Progress / 100.0;
                        if (!msgObject.IsIndeterminate)
                        {
                            this.GlobalBusyIndicator.BusyContent = msgObject.Message.IsNullOrEmpty() ? msgObject.Progress.ToString() : msgObject.Message;
                        }
                        else
                        {
                            this.GlobalBusyIndicator.BusyContent = "Loading";
                        }

                        if (msgObject.IsFinished)
                        {
                            currentSender = null;
                            ContinueToProgressMessages();
                        }
                    }
                    else
                    {
                        if (msgObject.Sender.IsNotNull())
                        {
                            allNotFinishedProgressMessages.AddExtension(msgObject.Sender, new List<ProgressMessageObject>(), false);
                            allNotFinishedProgressMessages[msgObject.Sender].Add(msgObject);
                        }
                    }
                }));
        }

        private void ContinueToProgressMessages()
        {
            if (this.allNotFinishedProgressMessages.Count > 0)
            {
                var keyvalue = this.allNotFinishedProgressMessages.FirstOrDefault();
                var group = keyvalue.Value.GroupBy(msg => msg.Progress).LastOrDefault();
                var pmsg = group.IsNotNull() ? group.FirstOrDefault() : null;
                this.allNotFinishedProgressMessages.Remove(keyvalue.Key);
                if (pmsg.IsNotNull())
                {
                    SendProgressMessage(pmsg);
                }
                
            }
        }
        #endregion
    }
}
