using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Linq;

namespace MachinaAurum.RTPS.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var queue = new ConcurrentQueue<Message>();

            var writer = new Writer(queue);
            var writerHistoryCache = new HistoryCache();

            var reader = new Reader(queue);

            var id = new GuidInstanceHandle(Guid.NewGuid());

            var newChange = writer.NewChange(ChangeKind.Alive, new IntData(0), id);
            writerHistoryCache.AddChange(newChange);

            writer.Cancel();
            await writer.Completion;

            Message msg = null;
            queue.TryDequeue(out msg);

            Assert.AreEqual(ProtocolId.ProtocolRTPS, msg.Header.Protocol);
            Assert.AreEqual(ProtocolVersion.v22, msg.Header.Version);
            Assert.AreEqual(VendorId.Unknown, msg.Header.Vendor);
            Assert.AreEqual(Guid.Empty, msg.Header.GuidPrefix.Prefix);

            Assert.AreEqual(1, msg.SubMessages.Count());

            var subMessage = msg.SubMessages.First();

            Assert.AreEqual(SubMessageFlag.HighEndian, subMessage.Header.Flags);
            Assert.AreEqual(SubMessageKind.Data, subMessage.Header.SubMessageId);
            Assert.AreEqual(4, subMessage.Header.SubMessageLength);
        }
    }
}
