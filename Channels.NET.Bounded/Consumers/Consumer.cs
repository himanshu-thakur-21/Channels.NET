using Channels.NET.Bounded.ChannelData;
using Channels.NET.Bounded.Loggers;
using System.Threading.Channels;

namespace Channels.NET.Bounded.Consumers
{
    public sealed class Consumer
    {
        private readonly ChannelReader<Envelope> _reader;
        private readonly int _consumerId;

        public Consumer(ChannelReader<Envelope> reader, int consumerId)
        {
            _reader = reader;
            _consumerId = consumerId;
        }

        public async Task BeginConsumeAsync(CancellationToken cancellationToken = default)
        {
            Logger.Log($"Consumer: {_consumerId} > starting", ConsoleColor.Green);

            try
            {
                await foreach (var message in _reader.ReadAllAsync(cancellationToken))
                {
                    Logger.Log($"CONSUMER: {_consumerId} > Received message: {message.Payload}", ConsoleColor.Green);
                    await Task.Delay(500, cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                Logger.Log($"Consumer: {_consumerId} > force stopped", ConsoleColor.Green);
            }

            Logger.Log($"Consumer: {_consumerId} > shutting down", ConsoleColor.Green);
        }
    }
}
