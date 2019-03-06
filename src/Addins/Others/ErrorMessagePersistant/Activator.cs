using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIShell.OSGi;
using System.IO;
using Lusa.AddinEngine.Extension;
using Lusa.UI.Msic.FileService;
using Lusa.UI.Msic.MessageService;
using Lusa.UI.Msic.MessageService.MessageObject;

namespace ErrorMessagePersistant
{
    public class Activator : IBundleActivator,IMessageListener
    {
        public void Start(IBundleContext context)
        {
            MessageService.Instance.RegisterMessageListener(this);
        }

        public void Stop(IBundleContext context)
        {
            FlushMessages();
        }

        bool IMessageListener.CanProcess(MessageObject msg)
        {
            if(msg.IsNotNull() && (msg.Type == MessageType.ERROR || msg.Type == MessageType.FATAL))
            {
                return true;
            }
            return false;
        }

        private List<MessageObject> msgs = new List<MessageObject>();
        void IMessageListener.NotifyMessge(MessageObject msg)
        {
            msg.AddData("DataTime", DateTime.Now);
            msgs.Add(msg);
            if (msgs.Count >= 1000)
            {
                try
                {
                    var persistantMsg = msgs.GetRange(0,1000);

                    FlushMessages(persistantMsg);

                    msgs.RemoveRange(0,1000);
                }
                catch (Exception ex)
                {
                    MessageService.Instance.SendMessage(ex);
                }
               
            }
        }

        private void FlushMessages()
        {
            FlushMessages(msgs);
        }

        private void FlushMessages(List<MessageObject> messages)
        {
            if (messages.Any())
            {
                var allMessageLines = string.Empty;
                messages.ForEach(msg => allMessageLines += msg.Format(msg.GetData<DateTime>("DataTime")) + Environment.NewLine);

                var localTime = DateTime.Now;
                var fileName = localTime.ToString("HH_mm_ss_-yyyy_MM_dd") + @".txt";
                FileService.Instance.WriteContent(@"Error\" + fileName, allMessageLines);
            }
        }
    }
}
