using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using Infragistics.Windows.DockManager;
using Infragistics.Windows.DockManager.Events;
using Lusa.AddinEngine.Extension;
using Lusa.UI.MainPanel.Dock.View;
using Lusa.UI.Msic.FileService;
using Lusa.UI.Msic.MessageService;
using Lusa.UI.Msic.MessageService.MessageObject;
using Lusa.UI.WorkBenchContract.ExtensionPoint;
using Lusa.UI.WorkBenchContract.UI;
using Lusa.UI.WorkBenchContract.UI.Commands;
using Lusa.UI.WorkBenchContract.UI.Controls.Pane;
using Microsoft.Win32;

namespace Lusa.UI.MainPanel.Dock
{
    /// <summary>
    /// DesignerDockPanel.xaml 的交互逻辑
    /// </summary>
    public partial class MainDockPanel
    {
        private const string UISettingDataPath = @"PanView\Settings.xml";
        public MainDockPanel()
        {
            Infragistics.Themes.ThemeManager.ApplicationTheme = new Infragistics.Themes.IgTheme();
            InitializeComponent();
            _dockManager = DockManager;
        }

        void DesignerDockPanel_Loaded(object sender, RoutedEventArgs e)
        {
            MessageService.Instance.SendMessage(new MessageObject("All Views Loaded...."));
            MessageService.Instance.SendMessage(new MessageObject("All Views Loaded....", MessageType.WARNING));
            MessageService.Instance.SendMessage(new MessageObject("All Views Loaded....", MessageType.ERROR));
        }

        private static XamDockManager _dockManager;
        internal static XamDockManager XamDockManager
        {
            get { return _dockManager; }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            //LoadPaneViews();
            try
            {
                LoadPaneViews();
                SetupLayout();
            }
            catch (Exception ex)
            {
                MessageService.Instance.SendMessage(ex);
            }
        }

        public override void OnApplyTemplate()
        {

            base.OnApplyTemplate();

        }

        private void SetupLayout()
        {
            if (!FileService.Instance.IsExistFile(DefaultLayoutPath))
            {
                FileService.Instance.WriteContent(DefaultLayoutPath, this.DockManager.SaveLayout());
            }
            else if (FileService.Instance.IsExistFile(LayoutPath))
            {
                this.DockManager.LoadLayout(FileService.Instance.ReadContent(LayoutPath));
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

            allContents.Where(item => !item.Location.HasValue).ForEach(item => this.docPart.Items.Add(item.Pane));
            allContents.Where(item => item.Location.HasValue && item.Location.Value == DockLocation.Left)
                .ForEach(item => this.leftPart.Items.Add(item.Pane));
            allContents.Where(item => item.Location.HasValue && item.Location.Value == DockLocation.Right)
                .ForEach(item => this.rightPart.Items.Add(item.Pane));
            allContents.Where(item => item.Location.HasValue && item.Location.Value == DockLocation.Bottom)
                .ForEach(item => this.bottomPart.Items.Add(item.Pane));

            allContents.ForEach(
                item => item.DescriptorProvider.As<IUIContentRequest>(resquest => resquest.Content = item.Pane.Content));
        }

        private void AddDefaultPaneView(ICollection<IPaneViewDescriptorProvider> allProviders)
        {
            allProviders.Add(new OutputPaneProvider());
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
                    item.Pane.Image = new BitmapImage(new Uri(ImageFilePathProvider.GetAssemblyLocalPath(paneViewDescriptorProvider.GetType().Assembly, paneViewDescriptor.ImageUrl)));
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
                item.Pane = new CustomPane() { Header = paneViewDescriptor.Header, IsPinned = paneViewDescriptor.IsPinned, PaneItem = item };
                item.Pane.Loaded += Pane_Loaded;
                item.Pane.RequestBringIntoView += Pane_RequestBringIntoView;
                //item.Pane.Content = paneViewDescriptor.Content ??
                //                    System.Activator.CreateInstance(paneViewDescriptor.ContentType);
                if (!paneViewDescriptor.IsDocument)
                {
                    item.Location = paneViewDescriptor.Location;
                }
                item.Pane.AllowDocking = true;
                item.Pane.AllowDockingBottom = true;
                item.Pane.AllowDockingFloating = true;
                item.Pane.AllowDockingInTabGroup = true;
                item.Pane.AllowDockingLeft = true;
                item.Pane.AllowDockingRight = true;
                item.Pane.AllowInDocumentHost = true;
                item.Pane.AllowPinning = true;
                item.Descriptor = paneViewDescriptor;
                item.Pane.SerializationId = paneViewDescriptor.Id;
                item.Pane.Name = paneViewDescriptor.Name;
                item.Pane.Tag = item.Descriptor;

                return item;
            }
            return null;
        }

        void Pane_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {

        }

        void Pane_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private class ContentPaneItem
        {
            public PaneViewDescriptor Descriptor { get; set; }
            public IPaneViewDescriptorProvider DescriptorProvider { get; set; }
            public ContentPane Pane { get; set; }
            public DockLocation? Location { get; set; }
        }

        private void DockManager_OnInitializePaneContent(object sender, InitializePaneContentEventArgs e)
        {

        }

        private const string LayoutPath = @"Layout\Layout.xml";
        private const string DefaultLayoutPath = @"Layout\DefaultLayout.xml";

        private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ViewCommands.LoadLayout)
            {
                var layoutContent = FileService.Instance.ReadContent(LayoutPath);
                if (layoutContent.IsNotNullOrEmpty())
                {
                    this.DockManager.LoadLayout(layoutContent);
                }
            }
            else if (e.Command == ViewCommands.SaveLayout)
            {
                FileService.Instance.WriteContent(LayoutPath, this.DockManager.SaveLayout());
            }
            else if (e.Command == ViewCommands.ResetLayout)
            {
                var layoutContent = FileService.Instance.ReadContent(DefaultLayoutPath);
                if (layoutContent.IsNotNullOrEmpty())
                {
                    this.DockManager.LoadLayout(layoutContent);
                }
            }
            else if (e.Command == ViewCommands.LoadSetings)
            {
                var openFile = new OpenFileDialog();
                openFile.Filter = "(*.xml)|*.xml";
                openFile.InitialDirectory = System.IO.Path.GetDirectoryName(FileService.Instance.GetAbsolutePath(UISettingDataPath));
                var result = openFile.ShowDialog();
                if (result.HasValue && result.Value)
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
                            xroot.Add(new XElement(XName.Get(s.Item.Pane.SerializationId), content));
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
                            if (allDatas.ContainsKey(s.Item.Pane.SerializationId))
                            {
                                var value = allDatas[s.Item.Pane.SerializationId];
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

        private void CommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private class OutputPaneProvider : IPaneViewDescriptorProvider
        {

            PaneViewDescriptor IPaneViewDescriptorProvider.Pane
            {
                get { return new PaneViewDescriptor(typeof(Output)) { Header = "Output", Location = DockLocation.Bottom, IsPinned = true, ImageUrl = @"/Images/output.png" }; }
            }
        }

        private class CustomPane : ContentPane
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

            protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
            {
                BuildContent();
                base.OnRender(drawingContext);
            }
        }
    }

}
