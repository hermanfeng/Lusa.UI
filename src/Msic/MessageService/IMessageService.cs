using System.Collections.Generic;

namespace Lusa.UI.Msic.MessageService
{
    public interface IMessageService
    {
        bool EnableMessage { get; set; }
        void SendMessage(MessageObject.MessageObject msg);
        void RegisterMessageListener(IMessageListener msgListener);
    }


    public interface IMessageListener
    {
        bool CanProcess(MessageObject.MessageObject msg);
        void NotifyMessge(MessageObject.MessageObject msg);
    }

    public interface IMessageProvider
    {
        IEnumerable<MessageObject.MessageObject> AllMessageObjects { get; }
    }
}
