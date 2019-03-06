using System.Collections.Generic;
using System;

namespace CommonLibrary
{
    public class DefaultMessageAssistant : IMessageAssistant
    {
        private readonly string _linkMsg = string.Empty;

        public DefaultMessageAssistant(string linkMsg, Action<MessageRange> clickAction = null)
            : this(clickAction)
        {
            _linkMsg = linkMsg;
        }

        private Action<MessageRange> _clickAction;
        public DefaultMessageAssistant(Action<MessageRange> clickAction = null)
        {
            _clickAction = clickAction;
        }

        protected string LinkMessage
        {
            get { return _linkMsg; }
        }


        IEnumerable<MessageRange> IMessageAssistant.GetAllMessageRanges(MessageObject msg)
        {
            return GetAllMessageRangesCore(msg);
        }

        bool IMessageClickAssistant.OnMessageRangeClick(MessageRange range)
        {
            return OnMessageRangeClickCore(range);
        }

        protected virtual bool OnMessageRangeClickCore(MessageRange range)
        {
            if (_clickAction != null)
            {
                _clickAction(range);
            }
            return false;
        }

        protected virtual IEnumerable<MessageRange> GetAllMessageRangesCore(MessageObject msg)
        {
            return MessageRange.FromMessage(msg, _linkMsg);
        }
    }
}
