using Channels.NET.Bounded.ChannelData;
using Channels.NET.Bounded.Consumers;
using Channels.NET.Bounded.Loggers;
using Channels.NET.Bounded.Producers;
using System.Threading.Channels;

// a channel is a data structure to store data from a Producer, which can then be consumed by one or more Consumers.
// unlike a Publisher/Subscriber (Pub/Sub) model, where a message produced can be received by one or more Subscribers concurrently,
// channels only allow for one Consumer to read a given message.

// Main Program
{
    // single producer, single consumer
    // await Run(1, 10, 1, 1);

    // single producer, multiple consumers
     // await Run(1, 10, 1, 3);

    // multiple producers, multiple consumers
    await Run(100, 70, 3, 3);

    Logger.Log("Done!");
    Console.ReadLine();
}

static async Task Run(int maxMessagesInChannel, int messagesToSend, int producersCount, int consumersCount)
{
    Logger.Log("*** STARTING EXECUTION ***");
    Logger.Log($"producers #: {producersCount}, buffer size: {maxMessagesInChannel}, consumers #: {consumersCount}");

    var channel = Channel.CreateBounded<Envelope>(maxMessagesInChannel);

    var tokenSource = new CancellationTokenSource();    
    var cancellationToken = tokenSource.Token;

    var tasks = new List<Task>(StartConsumers(channel, consumersCount, cancellationToken))
    {
        ProduceAsync(channel, messagesToSend, producersCount, tokenSource)
    };

    await Task.WhenAll(tasks);
    Logger.Log("*** EXECUTION COMPLETE ***");
}

static Task[] StartConsumers(Channel<Envelope> channel, int consumersCount, CancellationToken cancellationToken)
{
    return Enumerable.Range(1, consumersCount)
        .Select(i => new Consumer(channel.Reader, i).BeginConsumeAsync(cancellationToken))
        .ToArray();
}

static async Task ProduceAsync(Channel<Envelope> channel,
    int messagesCount,
    int producersCount,
    CancellationTokenSource source)
{
    var producers = Enumerable.Range(1, producersCount)
        .Select(i => new Producer(channel.Writer, i))
        .ToArray();

    int index = 0;
    var tasks = Enumerable.Range(1, messagesCount)
        .Select(i =>
        {
            index = ++index % producersCount;
            var producer = producers[index];
            var message = new Envelope($"Message {i}");

            return producer.PublishAsync(message, source.Token);
        });

    await Task.WhenAll(tasks);

    Logger.Log("Done publishing, closing writer");
    channel.Writer.Complete();

    Logger.Log("Waiting for consumer to complete...");
    await channel.Reader.Completion;

    Logger.Log("Consumers done processing, shutting down...");
    source.Cancel();
}

// More info on how we can setup the 'Async Data Pipelines' using c# channels
// https://deniskyashif.com/2020/01/07/csharp-channels-part-3/
