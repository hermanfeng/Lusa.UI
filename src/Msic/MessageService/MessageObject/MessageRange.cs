using System.Collections.Generic;
using System.Linq;
using Lusa.AddinEngine.Extension;

namespace Lusa.UI.Msic.MessageService.MessageObject
{
    public class MessageRange
    {
        public MessageObject OwnerMessage { get; set; }
        public MessageRange(MessageObject msg)
        {
            OwnerMessage = msg;
            msg.CheckNull("MessageObject");
            msg.Message.CheckNull("MessageObject.Message");
        }

        public int StartIndex { get; set; }

        public int Length { get; set; }

        public int EndIndex
        {
            get { return StartIndex + Length; }
            
        }

        public string Message
        {
            get
            {
                if (EndIndex >= StartIndex && StartIndex >= 0 && OwnerMessage.Message.Length >= (Length + StartIndex))
                {
                    return OwnerMessage.Message.Substring(StartIndex, Length);
                }
                return string.Empty;
            }
        }

        public static IEnumerable<MessageRange> FromMessage(MessageObject msg, string linkMsg)
        {
            var ranges = new List<MessageRange>();

            if (msg.IsNotNull() && msg.Message.IsNotNullOrEmpty() && linkMsg.IsNotNullOrEmpty())
            {
                FindAllRanges(msg, linkMsg,0, ranges);
            }

            return ranges;
        }

        public static MessageRange FromMessageSingle(MessageObject msg, string linkMsg)
        {
            var ranges = new List<MessageRange>();

            if (msg.IsNull() && msg.Message.IsNotNullOrEmpty() && linkMsg.IsNotNullOrEmpty())
            {
                FindAllRanges(msg, linkMsg, 0, ranges, true);
            }

            return ranges.FirstOrDefault();
        }

        private static void FindAllRanges(MessageObject msg, string linkMsg, int startindex, ICollection<MessageRange> ranges,bool justOneMatch=false)
        {
            var matchedIndex = msg.Message.IndexOf(linkMsg, startindex, System.StringComparison.Ordinal);
            if (matchedIndex >= 0)
            {
                var msgRange = new MessageRange(msg);
                msgRange.StartIndex = matchedIndex;
                msgRange.Length = linkMsg.Length;
                ranges.Add(msgRange);
                matchedIndex = matchedIndex + linkMsg.Length;
                if (matchedIndex < msg.Message.Length && !justOneMatch)
                {
                    FindAllRanges(msg, linkMsg, matchedIndex, ranges);
                }
            }
        }
    }
}
