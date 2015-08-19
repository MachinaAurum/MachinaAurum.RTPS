using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace MachinaAurum.RTPS.Tests
{
    internal class Writer
    {
        public Guid Id { get; private set; }
        SequenceNumber SequenceNumber;

        public Task Completion { get; private set; }

        BlockingCollection<CacheChange> Queue;
        private IDuplexChannel<Message> Channel;
        
        public Writer(IDuplexChannel<Message> channel)
        {
            Channel = channel;

            Id = Guid.NewGuid();
            SequenceNumber = new SequenceNumber();                        
        }

        internal CacheChange NewChange(ChangeKind kind, Data data, InstanceHandle instance)
        {
            var change = new CacheChange()
            {
                WriterGuid = Id,
                SequenceNumber = SequenceNumber.Increment(),

                Kind = kind,
                InstanceHandle = instance,

                Data = data
            };

            var message = new Message()
            {
                Header = new Header()
                {
                    Protocol = ProtocolId.ProtocolRTPS,
                    Version = ProtocolVersion.v22,
                    Vendor = VendorId.Unknown,
                    GuidPrefix = new GuidPrefix()
                }
            };

            message.AddSubMessage(new DataSubMessage(data));
            message.AddSubMessage(new HeartbeatSubMessage());

            Channel.Send(message);

            return change;
        }

        internal void Cancel()
        {
            Queue.CompleteAdding();
        }
    }
}