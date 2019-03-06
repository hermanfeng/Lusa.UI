using System.Collections.Generic;
using UIShell.OSGi;
using AddinEngine;
using System;
using System.Linq;

namespace CommonLibrary
{
    public interface IMessageAssistant : IMessageClickAssistant
    {
        IEnumerable<MessageRange> GetAllMessageRanges(MessageObject msg);
    }

    public interface IMessageClickAssistant
    {
        bool OnMessageRangeClick(MessageRange range);
    }
}
