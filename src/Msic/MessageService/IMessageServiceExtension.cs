using System;
using System.Windows.Media;
using Lusa.AddinEngine.Extension;
using Lusa.UI.Msic.MessageService.MessageObject;

namespace Lusa.UI.Msic.MessageService
{
    public static class IMessageServiceExtension
    {
        public static void SendMessage(this IMessageService service, string message
            , MessageType type = MessageType.INFO, object sender = null, bool needPopup=false, Color? customColor=null)
        {
            if (service != null)
            {
                service.SendMessage(new MessageObject.MessageObject(message) { Type = type, Sender = sender,NeedPopup = needPopup, CustomColor = customColor});
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="message"></param>
        /// <param name="clickAction">the action when click the clickmessage</param>
        /// <param name="clickMessage">part of message that display as a link  </param>
        /// <param name="type"></param>
        public static void SendHyperLinkMessage(this IMessageService service, string message, string clickMessage = "", Action<MessageRange> clickAction = null, MessageType type = MessageType.INFO)
        {
            if (service != null)
            {
                clickMessage = clickMessage.IsNullOrEmpty() ? message : clickMessage;
                service.SendMessage(new MessageObject.MessageObject(message) { Type = type, Sender = new DefaultMessageAssistant(clickMessage, clickAction) });
            }
        }

        public static void SendMessage(this IMessageService service, Exception e, MessageType type = MessageType.ERROR, object sender = null)
        {
            if (service != null && e!=null)
            {
                var message = BuildExceptionMessage(e);
                service.SendMessage(new MessageObject.MessageObject(message) { Type = type, Sender = sender });
            }
        }

        private static string BuildExceptionMessage(Exception e)
        {
            var message = e.Message + Environment.NewLine + e.StackTrace;
            if (e.InnerException.IsNotNull())
            {
                message += Environment.NewLine +"InnerException : " + BuildExceptionMessage(e.InnerException);
            }
            return message;
        }

        public static void SendProgressMessage(this IMessageService service, int progress,object sender=null,string message="", bool isIndeterminate=false)
        {
            if (service != null)
            {
                var msg = new ProgressMessageObject() { Progress = progress, Message= !string.IsNullOrEmpty(message) ? message+$"({progress}%)" : string.Empty, Sender =sender, IsIndeterminate = isIndeterminate };
                service.SendMessage(msg);
            }
        }


    }

}
