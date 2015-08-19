using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MachinaAurum.RTPS.Tests
{
    internal abstract class SubMessage
    {
        ICollection<SubMessageElement> _Elements;

        public SubMessageHeader Header { get; set; }

        public IEnumerable<SubMessageElement> Elements { get { return _Elements; } }

        public SubMessage()
        {
            _Elements = new Collection<SubMessageElement>();
        }

        internal void AddSubMessageElement(SubMessageElement element)
        {
            _Elements.Add(element);
        }
    }    
}