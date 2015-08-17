using System;

namespace MachinaAurum.RTPS.Tests
{
    internal class CacheChange
    {
        public ChangeKind Kind { get; set; }
        public Guid WriterGuid { get; set; }
        public InstanceHandle InstanceHandle { get; set; }
        public SequenceNumber SequenceNumber { get; set; }

        public Data Data { get; set; }
    }
}