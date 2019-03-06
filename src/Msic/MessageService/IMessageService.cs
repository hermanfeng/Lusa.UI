using System.Collections.Generic;

namespace CommonLibrary
{
    public interface IMessageService
    {
        bool EnableMessage { get; set; }
        void SendMessage(MessageObject msg);
        void RegisterMessageListener(IMessageListener msgListener);
    }


    public interface IMessageListener
    {
        bool CanProcess(MessageObject msg);
        void NotifyMessge(MessageObject msg);
    }

    public interface IMessageProvider
    {
        IEnumerable<MessageObject> AllMessageObjects { get; }
    }
}
