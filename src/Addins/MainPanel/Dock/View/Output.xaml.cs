using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;
using Infragistics.Controls.Editors;
using Infragistics.Controls.Editors.Primitives;
using Infragistics.Controls.Menus;
using Infragistics.Documents.RichText;
using Lusa.AddinEngine.Extension;
using Lusa.UI.Msic.MessageService;
using Lusa.UI.Msic.MessageService.MessageObject;
using Lusa.UI.WorkBenchContract.Extension;

namespace Lusa.UI.MainPanel.Dock.View
{
    /// <summary>
    /// Interaction logic for Output.xaml
    /// </summary>
    public partial class Output : IMessageListener
    {
        private RichTextDocument document = new RichTextDocument();
        private Dictionary<MessageObject, ParagraphNode> messageElements = new Dictionary<MessageObject, ParagraphNode>();
        private Dictionary<string, MessageRange> messageRanges = new Dictionary<string, MessageRange>(); 
        private ObservableCollection<string> _supportedOutputFormats = new ObservableCollection<string>();
        public Output()
        {
            InitializeComponent();
            MessageService.Instance.RegisterMessageListener(this);
            InitializeDocument();
            SetupMessgeFilter();
        }

        private void FindScrollBar()
        {
            if (this.scrollBar == null)
            {
                var area = this.xamRichTextEditor1.FindFirstVisualChild<RichDocumentViewScrollBarArea>();
                if (area.IsNotNull())
                {
                    this.scrollBar = area.FindFirstVisualChild<ScrollBar>();
                }
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            try
            {
                var msgProvider = MessageService.Instance.As<IMessageProvider>();
                if (msgProvider.IsNotNull())
                {
                    msgProvider.AllMessageObjects.ForEach(msg =>
                    {
                        if (!messageElements.ContainsKey(msg))
                        {
                            OutputOneMessage(msg);
                        }
                    });
                }
            }
            catch
            {
                //ignore
            }
        }

        public ObservableCollection<string> SupportedOutputFormats
        {
            get
            {
                return this._supportedOutputFormats;
            }
        }

        ScrollBar scrollBar;
        private void InitializeDocument()
        {
            document.IsReadOnly = true;
           
            this.document.RootNode.ChildNodes[0].ChildNodes.Clear();

            this.xamRichTextEditor1.Document = document;

            this.xamRichTextEditor1.HyperlinkExecuting += xamRichTextEditor1_HyperlinkExecuting;

            //set default link style
            if (!this.xamRichTextEditor1.Document.RootNode.Styles.Contains("Hyperlink"))
            {
                this.xamRichTextEditor1.Document.RootNode.Styles.Add(
                    this.xamRichTextEditor1.Document.AvailableStyles["Hyperlink"].Clone() as CharacterStyle);
            }

           
        }

        public bool IsVerticalScrollBarAtButtom
        {
            get
            {
                if(this.scrollBar.IsNull())
                {
                    return true;
                }


                double dVer = scrollBar.Maximum;

                return Math.Abs(scrollBar.Maximum - scrollBar.Value)< scrollBar.SmallChange *5;
            }
        } 

        

        private void SetupMessgeFilter()
        {
            this._supportedOutputFormats.Add("ALL");
            Enum.GetNames(typeof(MessageType)).ForEach(type => this._supportedOutputFormats.Add(type));
            this.outputType.SelectedIndex = 0;
        }

        bool IMessageListener.CanProcess(MessageObject msg)
        {
            return msg.Type != MessageType.Progress;
        }

        void IMessageListener.NotifyMessge(MessageObject msg)
        {
            OutputOneMessage(msg);
        }

        private void OutputOneMessage(MessageObject msg)
        {
            this.xamRichTextEditor1.Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    var settings = SetupMessageDisplaySetting(msg);

                    var pn = CreateParagraph(settings, msg);

                    pn.ApplySettings(SetupParagraphSettings(msg));

                    if (messageElements.Count == 0)
                    {
                        TextNodes.Clear();
                    }

                    messageElements.Add(msg, pn);

                    if (CanOutput(msg))
                    {
                        if (TextNodes.Count > MaxMessageDisplayCount * 2)
                        {
                            TextNodes.RemoveRange(0, MaxMessageDisplayCount);
                        }

                        var parent = TextNodes.Parent;
                        TextNodes.Add(pn);
                    }

                    ScrowToLastLine();
                }
                catch(Exception ex)
                {

                }
            }), DispatcherPriority.Normal);
        }

        private int MaxMessageDisplayCount = 500;

        private ParagraphNode CreateParagraph(CharacterSettings cs, MessageObject msg)
        {
            var pn = new ParagraphNode();

            var messageAssistant = msg.Sender as IMessageAssistant;
            if (messageAssistant.IsNotNull())
            {
                var allMessageRanges = messageAssistant.GetAllMessageRanges(msg);
                if (allMessageRanges.IsNotNull())
                {
                    BuildLinkNodeParagraph(cs, msg, pn, allMessageRanges);
                }
            }
            else
            {
                var message = FormatMessgae(msg);
                var rn = CreateNormalMessageNode(cs, message);
                pn.ChildNodes.Add(rn);
            }

            return pn;
        }

        private void BuildLinkNodeParagraph(CharacterSettings cs, MessageObject msg, ParagraphNode pn,
            IEnumerable<MessageRange> allMessageRanges)
        {
            pn.ChildNodes.Add(CreateNormalMessageNode(cs, string.Format(@"[{0}] [{1}] : ", msg.Type, DateTime.Now.ToString("HH:mm:ss"))));
            allMessageRanges.ForEach((range, lastRange) =>
            {
                int lastIndex = 0;
                if (lastRange.IsNotNull())
                {
                    lastIndex = lastRange.EndIndex;
                }

                if (range.StartIndex > lastIndex)
                {
                    var normalMsg = msg.Message.Substring(lastIndex, range.StartIndex - lastIndex);
                    pn.ChildNodes.Add(CreateNormalMessageNode(cs, normalMsg));
                }

                var guid = Guid.NewGuid().ToString();
                this.messageRanges.Add(guid, range);
                var linkNode = CreatedLinkNode(range.Message, guid);
                pn.ChildNodes.Add(linkNode);
            });
        }

        private string FormatMessgae(MessageObject msg)
        {
            var result = msg.Format(DateTime.Now);
            if (result.EndsWith(Environment.NewLine))
            {
                result = result.Substring(0, result.Length - Environment.NewLine.Length);
            }
            return result;
        }

        private static RunNode CreateNormalMessageNode(CharacterSettings cs, string message)
        {
            var rn = new RunNode();
            rn.SetText(message);
            rn.Settings = cs;
            return rn;
        }

        private HyperlinkNode CreatedLinkNode(string linkText, string linkUri,string linkTips="")
        {
            var hNode = new HyperlinkNode();
            hNode.Tooltip = linkTips;
            hNode.Uri = linkUri;
            hNode.SetText(linkText);
            hNode.TrackHistory = true;
            
            return hNode;
        }

        private void ScrowToLastLine()
        {

            if (this.xamRichTextEditor1.ActiveDocumentView != null && this.xamRichTextEditor1.ActiveDocumentView.VerticalScrollBarVisibility == System.Windows.Visibility.Visible)
            {
                FindScrollBar();
                if (IsVerticalScrollBarAtButtom)
                {
                    this.xamRichTextEditor1.ActiveDocumentView.VerticalViewOffset = double.MaxValue;
                }
            }
        }

        private static ParagraphSettings SetupParagraphSettings(MessageObject msg)
        {
            var psetting = new ParagraphSettings();
            psetting.Spacing = new ParagraphSpacingSettings()
            {
                AfterParagraph = new ParagraphVerticalSpacing(new Extent(0)),
                BeforeParagraph = new ParagraphVerticalSpacing(new Extent(0)),
                LineSpacing = new LineSpacing(new Extent(0), ExtentRule.AtLeast)
            };
            return psetting;
        }

        private static CharacterSettings SetupMessageDisplaySetting(MessageObject msg)
        {
            var settings = new CharacterSettings();
            if (msg.Type == MessageType.ERROR || msg.Type == MessageType.FATAL)
            {
                settings.Color = new ColorInfo(Color.FromRgb(255, 0, 0));
            }
            else if (msg.Type == MessageType.WARNING)
            {
                settings.Color = new ColorInfo(Color.FromRgb(0, 0, 255));
            }
            else if(msg.CustomColor.HasValue)
            {
                settings.Color = new ColorInfo(msg.CustomColor.Value);
            }

            return settings;
        }

        private NodeCollection TextNodes
        {
            get { return this.xamRichTextEditor1.Document.RootNode.Body.ChildNodes; }
        }

        private bool CanOutput(MessageObject msg)
        {
            if (this.outputType.SelectedItem.IsNull())
            {
                return false;
            }

            var seletedType = this.outputType.SelectedItem.ToString();
            return msg.Type.ToString() == seletedType || seletedType == "ALL";
        }

        private void UpdateMessages()
        {
            TextNodes.Clear();
            TextNodes.InsertRange(0, messageElements.Where(keyvalue => CanOutput(keyvalue.Key)).Select(keyvalue=>keyvalue.Value));
            ScrowToLastLine();
        }

        private void xamRichTextEditor1_HyperlinkExecuting(object sender, HyperlinkExecutingEventArgs e)
        {
            if (messageRanges.ContainsKey(e.Uri))
            {
                HandleLinkClickEvent(e);
            }
        }

        private void HandleLinkClickEvent(HyperlinkExecutingEventArgs e)
        {
            var msgRange = messageRanges[e.Uri];

            //handle by added MessageAssistant
            var handled =
                msgRange.OwnerMessage.Sender.As<IMessageAssistant, bool>(
                    assistant => assistant.OnMessageRangeClick(messageRanges[e.Uri]));

            //handle by Global assistant

            AllMessageClickAssistants.ForEach(assistant => { handled = assistant.OnMessageRangeClick(msgRange) | handled; });

            e.Cancel = handled;

            //recover the url and 
            if (!handled)
            {
                e.Uri = msgRange.Message;
            }
        }

        private List<IMessageClickAssistant> allMessageClickAssistants = new List<IMessageClickAssistant>();
        private IEnumerable<IMessageClickAssistant> AllMessageClickAssistants
        {
            get { return allMessageClickAssistants; }
        }

        private void XamRichTextEditor1_OnContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
           e.ContextMenu.Items.RemoveAt(4);
            var item = new XamMenuItem() {Header = "Clear ALL"};
            item.Click += ClearAll_click;
            e.ContextMenu.Items.Add(item);
        }

        void ClearAll_click(object sender, EventArgs e)
        {
            messageElements.Clear();
            UpdateMessages();
        }

        private void outputType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (this.outputType.SelectedItem.IsNotNull())
            {
                if (_supportedOutputFormats.Contains(this.outputType.SelectedItem.ToString()))
                {
                    UpdateMessages();
                }
            }
        }
    }
}