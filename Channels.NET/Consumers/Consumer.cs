using System.Threading.Channels;

namespace Channels.NET.Unbounded.Consumers
{
    public sealed class Consumer
    {
        private readonly ChannelReader<string> _channelReader;

        public Consumer(ChannelReader<string> channelReader)
        {
            _channelReader = channelReader;
        }

        public async Task ConsumeWorkAsync()
        {
            await foreach (var todoItem in _channelReader.ReadAllAsync())
            {
                Console.WriteLine($"Completing todo: {todoItem}");
                await Task.Delay(1500);
            }

            Console.WriteLine("All items read");
        }
    }
}
