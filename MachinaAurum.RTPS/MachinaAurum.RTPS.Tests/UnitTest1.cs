using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Collections.Concurrent;

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

            await Task.Delay(1000);

            reader.Cancel();
            await reader.Completion;
        }
    }
}
