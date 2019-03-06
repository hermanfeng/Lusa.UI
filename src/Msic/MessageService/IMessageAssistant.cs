using System.Collections.Generic;

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
