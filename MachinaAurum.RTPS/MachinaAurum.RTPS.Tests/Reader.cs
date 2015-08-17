using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace MachinaAurum.RTPS.Tests
{
    internal class Reader
    {
        private ConcurrentQueue<Message> queue;

        public Task Completion { get; private set; }
        CancellationTokenSource cancel;

        public Reader()
        {
            cancel = new CancellationTokenSource();

            Completion = Task.Factory.StartNew(async () =>
            {
                try {
                    while (true)
                    {
                        await Task.Delay(100, cancel.Token);

                        Message msg = null;
                        if (queue.TryDequeue(out msg))
                        {
                            Console.WriteLine("Reader");
                        }
                    }
                }
                catch(TaskCanceledException)
                {

                }
            });
        }

        public Reader(ConcurrentQueue<Message> queue) : this()
        {
            this.queue = queue;
        }

        public void Cancel()
        {
            cancel.Cancel();
        }
    }
}