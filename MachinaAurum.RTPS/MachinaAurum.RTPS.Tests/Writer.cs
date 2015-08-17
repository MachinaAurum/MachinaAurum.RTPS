using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace MachinaAurum.RTPS.Tests
{
    internal class Writer
    {
        Guid Id;
        SequenceNumber SequenceNumber;

        public Task Completion { get; private set; }

        BlockingCollection<CacheChange> Queue;
        private ConcurrentQueue<Message> queue;

        public Writer()
        {
            Id = Guid.NewGuid();
            SequenceNumber = new SequenceNumber();
            Queue = new BlockingCollection<CacheChange>();

            Completion = Task.Factory.StartNew(() =>
            {
                var items = Queue.GetConsumingEnumerable();

                foreach (var item in items)
                {
                    Console.WriteLine("Writer");

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

                    var subMessage = new SubMessage()
                    {
                        Header = new SubMessageHeader()
                        {
                            SubMessageId = SubMessageKind.Data,
                            Flags = SubMessageFlag.HighEndian,
                            SubMessageLength = 4
                        }
                    };

                    subMessage.AddSubMessageElement(new SerializedPaylodSubMessageElement(item.Data));

                    message.AddSubMessage(subMessage);

                    this.queue.Enqueue(message);
                }
            });
        }

        public Writer(ConcurrentQueue<Message> queue) : this()
        {
            this.queue = queue;
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

            Queue.Add(change);

            return change;
        }

        internal void Cancel()
        {
            Queue.CompleteAdding();
        }
    }
}