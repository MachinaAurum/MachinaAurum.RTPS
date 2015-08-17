using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MachinaAurum.RTPS.Tests
{
    internal class Message
    {
        ICollection<SubMessage> _SubMessages;

        public Header Header { get; set; }
        public IEnumerable<SubMessage> SubMessages
        {
            get
            {
                return _SubMessages;
            }
        }

        public Message()
        {
            _SubMessages = new Collection<SubMessage>();
        }

        internal void AddSubMessage(SubMessage msg)
        {
            _SubMessages.Add(msg);
        }
    }
}