using System.Threading.Channels;

namespace Channels.NET.Unbounded.Producers
{
    public sealed class Producer
    {
        private static readonly List<string> _todoItems = new()
        {
            "Make a coffee",
            "Read newspaper",
            "Go for a walk",
            "Have breakfast"
        };

        private readonly ChannelWriter<string> _channelWriter;

        public Producer(ChannelWriter<string> channelWriter)
        {
            _channelWriter = channelWriter;
        }

        public async Task ProduceTodoAsync()
        {
            foreach (var item in _todoItems)
            {
                await _channelWriter.WriteAsync(item);
                Console.WriteLine($"Added todo: '{item}' to channel");

                await Task.Delay(500);
            }

            _channelWriter.Complete();
        }
    }
}
