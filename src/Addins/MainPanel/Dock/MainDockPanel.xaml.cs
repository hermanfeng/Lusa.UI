using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CommonLibrary;
using MainPanelPlugin.View;
using CommonExtension;
using WorkBenchContract;
using Xceed.Wpf.AvalonDock;
using System.Xml;
using Xceed.Wpf.AvalonDock.Layout;
using Xceed.Wpf.AvalonDock.Layout.Serialization;
using System.IO;
using System.Xml.Linq;
using Microsoft.Win32;

namespace MainPanelPlugin
{
    /// <summary>
    /// DesignerDockPanel.xaml 的交互逻辑
    /// </summary>
    public partial class MainDockPanel:IMessageListener
    {
        public MainDockPanel()
        {
            InitializeComponent();
            MessageService.Instance.RegisterMessageListener(this);
            Dispatcher.UnhandledException += Dispatcher_UnhandledException;
            this.Unloaded += MainDockPanel_Unloaded;
        }


        private void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (this.GlobalBusyIndicator.IsBusy)
            {
                this.GlobalBusyIndicator.IsBusy = false;
            }
            e.Handled = true;
        }

        void DesignerDockPanel_Loaded(object sender, RoutedEventArgs e)
        {
            MessageService.Instance.SendMessage(new MessageObject("All Views Loaded...."));
            MessageService.Instance.SendMessage(new MessageObject("All Views Loaded....", MessageType.WARNING));
            MessageService.Instance.SendMessage(new MessageObject("All Views Loaded....", MessageType.ERROR));
        }

        private static DockingManager _dockManager;
        internal static DockingManager DockManager
        {
            
            get { return _dockManager; }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            //LoadPaneViews();
            _dockManager = dockingManager;
            try
            {
                LoadPaneViews();
                if(FileService.Instance.IsExistFile(LayoutPath))
                {
                    var layoutpath = FileService.Instance.EnsureAbsolutePath(LayoutPath);
                    LoadLayout(layoutpath);
                }
                var path = FileService.Instance.EnsureAbsolutePath(UISettingDataPath);
                LoadPanSetting(path);
            }
            catch (Exception ex)
            {
                MessageService.Instance.SendMessage(ex);
            }
        }

        private void MainDockPanel_Unloaded(object sender, RoutedEventArgs e)
        {
            var layoutpath = FileService.Instance.EnsureAbsolutePath(LayoutPath);
            SaveLayout(layoutpath);
            var settingfullPath = FileService.Instance.EnsureAbsolutePath(UISettingDataPath);
            SavePanSetting(settingfullPath);
        }

        private void SavePanSetting(string fullPath)
        {
            if (allContents != null)
            {
                var serializers = allContents.Where(item => item.Pane.Content.As<IPanViewSerializer>() != null).Select(item => new { Item = item, Serializer = item.Pane.Content as IPanViewSerializer }).ToList();
                if (serializers.Count > 0)
                {
                    var xroot = new XElement(XName.Get("Root"));
                    var doc = new XDocument(xroot);
                    foreach (var s in serializers)
                    {
                        try
                        {
                            var content = s.Serializer.Serializer();
                            xroot.Add(new XElement(XName.Get(s.Item.Pane.ContentId), content));
                        }
                        catch
                        {
                            //ingnore 
                        }
                    }
                    doc.Save(fullPath);
                }
            }
        }

        private void LoadPanSetting(string fullPath)
        {
            if (allContents != null)
            {
                if (File.Exists(fullPath))
                {
                    var serializers = allContents.Where(item => item.Pane.Content.As<IPanViewSerializer>() != null).Select(item => new { Item = item, Serializer = item.Pane.Content as IPanViewSerializer }).ToList();
                    if (serializers.Count > 0)
                    {
                        var doc = XDocument.Load(fullPath);
                        var allDatas = doc.Root.Elements().ToDictionary(e => e.Name.ToString(), e => e.Value);

                        foreach (var s in serializers)
                        {
                            if (allDatas.ContainsKey(s.Item.Pane.ContentId))
                            {
                                var value = allDatas[s.Item.Pane.ContentId];
                                if (!string.IsNullOrEmpty(value))
                                {
                                    s.Serializer.DeSerializer(value);
                                }
                            }
                        }
                    }
                }
            }
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
        }

        private void LoadLayout(string fullPath)
        {
            var serializer = new XmlLayoutSerializer(DockManager);
            using (var stream = new StreamReader(fullPath))
            {
                serializer.Deserialize(stream);
            }
        }

        private void SaveLayout(string fullPath)
        {
            var serializer = new XmlLayoutSerializer(DockManager);
            using (var stream = new StreamWriter(fullPath))
            {
                serializer.Serialize(stream);
            }
        }

        List<ContentPaneItem> allContents;
        private void LoadPaneViews()
        {
            var paneBuilder = new PaneViewPointBulder();

            allContents = new List<ContentPaneItem>();

            var providers = paneBuilder.BuildItems().GeneratedItems;

            AddDefaultPaneView(providers);

            providers.ForEach(provider =>
            {
                var pane = BuildLayoutContent(provider);
                if (pane != null)
                {
                    allContents.Add(pane);
                }
            });

            allContents.Where(item => !item.Location.HasValue).ForEach(item => this.docPart.Children.Add(item.Pane));
            allContents.Where(item => item.Location.HasValue && item.Location.Value == DockLocation.Left)
                .ForEach(item => this.leftPart.Children.Add(item.Pane));
            allContents.Where(item => item.Location.HasValue && item.Location.Value == DockLocation.Right)
                .ForEach(item => this.rightPart.Children.Add(item.Pane));
            allContents.Where(item => item.Location.HasValue && item.Location.Value == DockLocation.Bottom)
                .ForEach(item => this.bottomPart.Children.Add(item.Pane));

            //allContents.Where(item => item.Descriptor.IsPinned).ForEach(item =>
            //{
            //    item.Pane.ToggleAutoHide();
            //});

            allContents.ForEach(
                item => item.DescriptorProvider.As<IUIContentRequest>(resquest => resquest.Content = item.Pane.Content));
        }

        private void AddDefaultPaneView(ICollection<IPaneViewDescriptorProvider> allProviders)
        {
            allProviders.Add(new OutputPaneProvider());
            allProviders.Add(new UserSettingPaneProvider());
        }

        private ContentPaneItem BuildLayoutContent(IPaneViewDescriptorProvider paneViewDescriptorProvider)
        {
            var paneViewDescriptor = paneViewDescriptorProvider.Pane;
            if (paneViewDescriptor != null)
            {
                var item = BuildLayoutContent(paneViewDescriptor);
                item.DescriptorProvider = paneViewDescriptorProvider;

                if (paneViewDescriptor.ImageUrl.IsNotNullOrEmpty())
                {
                    item.Pane.IconSource = new BitmapImage(new Uri(ImageFilePathProvider.GetAssemblyLocalPath(paneViewDescriptorProvider.GetType().Assembly, paneViewDescriptor.ImageUrl)));
                }

                return item;
            }
            return null;
        }

        private ContentPaneItem BuildLayoutContent(PaneViewDescriptor paneViewDescriptor)
        {
            if (paneViewDescriptor != null)
            {
                var item = new ContentPaneItem();
                //item.Pane = new CustomPane() { Title = paneViewDescriptor.Header, IsPinned = paneViewDescriptor.IsPinned , PaneItem = item};
                item.Pane = new LayoutAnchorable() { Title = paneViewDescriptor.Header };
                item.Pane.Content = paneViewDescriptor.Content ??
                                    System.Activator.CreateInstance(paneViewDescriptor.ContentType);
                if (!paneViewDescriptor.IsDocument)
                {
                    item.Location = paneViewDescriptor.Location;
                }
                item.Pane.ContentId = paneViewDescriptor.Id;
                //item.Pane.AllowDocking = true;
                //item.Pane.AllowDockingBottom = true;
                //item.Pane.AllowDockingFloating = true;
                //item.Pane.AllowDockingInTabGroup = true;
                //item.Pane.AllowDockingLeft = true;
                //item.Pane.AllowDockingRight = true;
                //item.Pane.AllowInDocumentHost = true;
                //item.Pane.AllowPinning = true;
                item.Descriptor = paneViewDescriptor;
                //item.Pane.SerializationId = paneViewDescriptor.Id;
                item.Pane.Title = paneViewDescriptor.Name;
                //item.Pane.Tag = item.Descriptor;

                return item;
            }
            return null;
        }

        private class ContentPaneItem
        {
            public PaneViewDescriptor Descriptor { get; set; }
            public IPaneViewDescriptorProvider DescriptorProvider { get; set; }
            public LayoutAnchorable Pane { get; set; }
            public DockLocation? Location { get; set; }
        }

        private const string LayoutPath = @"Layout\Layout.xml";
        private const string UISettingDataPath = @"PanView\Settings.xml";

        private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ViewCommands.LoadLayout)
            {
                var openFile = new OpenFileDialog();
                openFile.Filter = "(*.xml)|*.xml";
                openFile.InitialDirectory = System.IO.Path.GetDirectoryName(FileService.Instance.GetAbsolutePath(LayoutPath));
                var result = openFile.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    this.LoadLayout(openFile.FileName);
                }
            }
            else if (e.Command == ViewCommands.SaveLayout)
            {
                var openFile = new SaveFileDialog();
                openFile.DefaultExt = ".xml";
                openFile.Filter = "(*.xml)|*.xml";
                openFile.InitialDirectory = System.IO.Path.GetDirectoryName(FileService.Instance.GetAbsolutePath(LayoutPath));
                var result = openFile.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    var fullPath = openFile.FileName;
                    this.SaveLayout(fullPath);
                }
            }
            //else if (e.Command == ViewCommands.ResetLayout)
            //{
            //    var layoutContent = FileService.Instance.ReadContent(DefaultLayoutPath);
            //    if (layoutContent.IsNotNullOrEmpty())
            //    {
            //        this.DockManager.LoadLayout(layoutContent);
            //    }
            //}
            else if (e.Command == ViewCommands.SaveSettings)
            {
                OpenFileDialog openFile = new OpenFileDialog();

            }
            else if (e.Command == ViewCommands.LoadSetings)
            {
                var openFile = new OpenFileDialog();
                openFile.Filter = "(*.xml)|*.xml";
                openFile.InitialDirectory = System.IO.Path.GetDirectoryName(FileService.Instance.GetAbsolutePath(UISettingDataPath));
                var result = openFile.ShowDialog();
                if(result.HasValue && result.Value)
                {
                    var fullPath = openFile.FileName;
                    LoadPanSetting(fullPath);
                }
            }
            else if (e.Command == ViewCommands.SaveAsSettings)
            {
                var openFile = new SaveFileDialog();
                openFile.DefaultExt = ".xml";
                openFile.Filter = "(*.xml)|*.xml";
                openFile.InitialDirectory = System.IO.Path.GetDirectoryName(FileService.Instance.GetAbsolutePath(UISettingDataPath));
                var result = openFile.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    var fullPath = openFile.FileName;
                    SavePanSetting(fullPath);

                }
            }
        }

        private void CommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


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
                //PopupDialogWindow(msg);
            }
            else
            {
                msg.As<ProgressMessageObject>(msgObject => SendProgressMessage(msgObject));
            }
        }

        private void SendProgressMessage(ProgressMessageObject msgObject)
        {
            this.Dispatcher.Invoke(new Action(() =>
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

        private class OutputPaneProvider : IPaneViewDescriptorProvider
        {

            PaneViewDescriptor IPaneViewDescriptorProvider.Pane
            {
                get { return new PaneViewDescriptor(typeof(Output)) { Header = "Output", Location = DockLocation.Bottom, IsPinned = true, ImageUrl = @"/Images/output.png"}; }
            }
        }

        private class UserSettingPaneProvider : IPaneViewDescriptorProvider
        {

            PaneViewDescriptor IPaneViewDescriptorProvider.Pane
            {
                get { return new PaneViewDescriptor(typeof(UserSettings)) { Header = "UserSettings", Location = DockLocation.Right, IsPinned = true, ImageUrl = @"/Images/output.png" }; }
            }
        }

        private class  CustomPane : LayoutAnchorable
        {
            public CustomPane()
            {
                
            }

            private void BuildContent()
            {
                if (PaneItem.IsNotNull() && PaneItem.Descriptor.IsNotNull() && Content == null)
                {
                    if (Content.IsNull())
                    {
                        Content = PaneItem.Descriptor.Content ?? System.Activator.CreateInstance(PaneItem.Descriptor.ContentType);
                    }
                    PaneItem.DescriptorProvider.As<IUIContentRequest>(resquest => resquest.Content = Content);
                }
            }

            public ContentPaneItem PaneItem { get; set; }

            

            //protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
            //{
            //    BuildContent();
            //    base.OnRender(drawingContext);
            //}
        }
    }

}
