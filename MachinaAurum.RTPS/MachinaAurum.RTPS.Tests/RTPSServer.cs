using MachinaAurum.Components.Sockets;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace MachinaAurum.RTPS.Tests
{
    internal class RTPSServer
    {
        ConcurrentDictionary<Guid, IDuplexChannel<Message>> Writers = new ConcurrentDictionary<Guid, IDuplexChannel<Message>>();
        Func<IDuplexChannel<Message>> CreateDuplex;

        public RTPSServer(Func<IDuplexChannel<Message>> createDuplex)
        {
            CreateDuplex = createDuplex;

            Task.Factory.StartNew(async () =>
            {
                var bufferPool = new BufferPool(100000);
                var awaitablePool = new AwaitableSocketPool(10000);

                var endpoint = new IPEndPoint(IPAddress.Any, 8080);
                var listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                listenSocket.Bind(endpoint);
                listenSocket.Listen(100);

                var allTasks = Enumerable.Range(0, 1)
                    .Select(x => Task.Factory.StartNew(() => AcceptConnections(listenSocket, CancellationToken.None)))
                    .ToArray();

                await Task.WhenAll(allTasks);
            });
        }

        public RTPSServer() : this(() => new QueueDuplexChannel<Message>())
        {
        }

        internal Writer NewWriter()
        {
            var channel = CreateDuplex();
            var writer = new Writer(channel);

            Writers.TryAdd(writer.Id, channel);

            return writer;
        }

        private async Task AcceptConnections(Socket socket, CancellationToken token)
        {
            var awaitable = new AwaitableSocket();

            while (true)
            {
                awaitable.Initialize();
                await awaitable.AcceptOnAsync(socket);

                if (token.IsCancellationRequested)
                {
                    break;
                }

                if (awaitable == null)
                {
                    break;
                }

                //Post(awaitable.Args.AcceptSocket);
            }
        }
    }

    public interface IDuplexChannel<T>
    {
        void Send(T item);
        Task<T> ReceiveAsync();
    }

    public class QueueDuplexChannel<T> : IDuplexChannel<T>
    {
        ConcurrentQueue<T> Queue = new ConcurrentQueue<T>();

        public async Task<T> ReceiveAsync()
        {
            while(true)
            {
                T item = default(T);

                if(Queue.Count > 0)
                {
                    if(Queue.TryDequeue(out item))
                    {
                        return item;
                    }
                }

                await Task.Delay(1000);
            }
        }

        public void Send(T item)
        {
            Queue.Enqueue(item);
        }
    }
}