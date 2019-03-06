using System.Collections.Generic;
using UIShell.OSGi;
using AddinEngine;
using System;
using System.Linq;

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
