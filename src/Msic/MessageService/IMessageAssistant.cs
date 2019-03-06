using System.Collections.Generic;
using Lusa.UI.Msic.MessageService.MessageObject;

namespace Lusa.UI.Msic.MessageService
{
    public interface IMessageAssistant : IMessageClickAssistant
    {
        IEnumerable<MessageRange> GetAllMessageRanges(MessageObject.MessageObject msg);
    }

    public interface IMessageClickAssistant
    {
        bool OnMessageRangeClick(MessageRange range);
    }
}
