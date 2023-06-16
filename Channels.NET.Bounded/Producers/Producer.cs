using Channels.NET.Bounded.ChannelData;
using Channels.NET.Bounded.Loggers;
using System.Threading.Channels;

namespace Channels.NET.Bounded.Producers
{
    public sealed class Producer
    {
        private readonly ChannelWriter<Envelope> _writer;
        private readonly int _producerId;

        public Producer(ChannelWriter<Envelope> writer, int instanceId)
        {
            _writer = writer;
            _producerId = instanceId;
        }

        public async Task PublishAsync(Envelope message, CancellationToken cancellationToken = default)
        {
            await _writer.WriteAsync(message, cancellationToken);
            Logger.Log($"Producer {_producerId} > published '{message.Payload}'", ConsoleColor.Yellow);
        }
    }
}