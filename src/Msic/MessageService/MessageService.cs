using System.Collections.Generic;
using AddinEngine;
using System;
using CommonExtension;

namespace CommonLibrary
{
    public class MessageService : IMessageService, IMessageProvider
    {
        internal MessageService()
        {
            System.AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        void CurrentDomain_UnhandledException(object sender, System.UnhandledExceptionEventArgs e)
        {
            e.ExceptionObject.As<Exception>(ex => IMessageServiceExtension.SendMessage(this,ex));
        }

        public static IMessageService Instance
        {
            get
            {

                var service = BundleActivatorBase.GetService<IMessageService>();
                if (service.IsNull())
                {
                    service = new MessageService();
                }
                return service;
            }
        }

        private List<MessageObject> allMessages = new List<MessageObject>();
        private List<IMessageListener> messageListeners = new List<IMessageListener>();
        public void SendMessage(MessageObject msg)
        {
            if (EnableMessage && msg != null)
            {
                allMessages.Add(msg);
                messageListeners.ForEach(listener =>
                {
                    if (listener.CanProcess(msg))
                    {
                        listener.NotifyMessge(msg);
                    }
                });
            }
        }

        public void RegisterMessageListener(IMessageListener msgListener)
        {
            if (msgListener!=null && !messageListeners.Contains(msgListener))
            {
                messageListeners.Add(msgListener);
            }
        }

        private bool enableMessgae = true;
        public bool EnableMessage
        {
            get { return enableMessgae; }
            set { enableMessgae = value; }
        }

        IEnumerable<MessageObject> IMessageProvider.AllMessageObjects
        {
            get { return allMessages; }
        }
    }

}
