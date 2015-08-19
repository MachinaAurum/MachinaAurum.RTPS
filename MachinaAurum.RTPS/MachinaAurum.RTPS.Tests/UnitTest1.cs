using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Sockets;

namespace MachinaAurum.RTPS.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var duplex = new QueueDuplexChannel<Message>();

            var server = new RTPSServer(() => duplex);

            var writer = server.NewWriter();
            var writerHistoryCache = new HistoryCache();

            var id = new GuidInstanceHandle(Guid.NewGuid());

            var newChange = writer.NewChange(ChangeKind.Alive, new IntData(0), id);
            writerHistoryCache.AddChange(newChange);

            var msg = await duplex.ReceiveAsync();

            Assert.AreEqual(ProtocolId.ProtocolRTPS, msg.Header.Protocol);
            Assert.AreEqual(ProtocolVersion.v22, msg.Header.Version);
            Assert.AreEqual(VendorId.Unknown, msg.Header.Vendor);
            Assert.AreEqual(Guid.Empty, msg.Header.GuidPrefix.Prefix);

            Assert.AreEqual(2, msg.SubMessages.Count());

            var subMessage1 = msg.SubMessages.Skip(0).First();
            var subMessage2 = msg.SubMessages.Skip(1).First();

            Assert.IsInstanceOfType(subMessage1, typeof(DataSubMessage));
            Assert.AreEqual(SubMessageFlag.HighEndian, subMessage1.Header.Flags);
            Assert.AreEqual(SubMessageKind.Data, subMessage1.Header.SubMessageId);
            Assert.AreEqual(4, subMessage1.Header.SubMessageLength);

            var element = subMessage1.Elements.First();

            Assert.IsInstanceOfType(element, typeof(SerializedPaylodSubMessageElement));

            Assert.IsInstanceOfType(subMessage2, typeof(HeartbeatSubMessage));
        }
    }
}
