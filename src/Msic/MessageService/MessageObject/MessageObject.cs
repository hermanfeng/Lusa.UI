using System;
using System.Windows.Media;
using Lusa.AddinEngine.ExtensionData;

namespace Lusa.UI.Msic.MessageService.MessageObject
{
    public class MessageObject:ExtensionDatas
    {
        public MessageObject()
        {
            Type = MessageType.INFO;
        }

        public MessageObject(string msg, MessageType type = MessageType.INFO, object sender = null, bool needPopup = false, Color? customColor = null)
        {
            this.Message = msg;
            this.Sender = sender;
            this.Type = type;
            this.NeedPopup = needPopup;
        }

        public string Format(DateTime outputTime)
        {
            return string.Format(@"[{0}] [{1}] : {2}", Type, outputTime.ToString("HH:mm:ss"), Message);
        }

        public Color? CustomColor { get; set; }

        public MessageType Type { get; set; }
        public string Message { get; set; }

        public bool NeedPopup { get; set; }

        public object Sender { get; set; }
    }

    public enum MessageType
    {
        INFO = 1,
        VERB = 2,
        Progress =3,
        DEBUG = 4,
        WARNING = 5,
        ERROR = 6,
        FATAL =7
        
    }


}
