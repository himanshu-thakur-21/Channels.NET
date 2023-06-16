using Channels.NET.Unbounded.Consumers;
using Channels.NET.Unbounded.Producers;
using System.Threading.Channels;

// a channel is a data structure to store data from a Producer, which can then be consumed by one or more Consumers.
// unlike a Publisher/Subscriber (Pub/Sub) model, where a message produced can be received by one or more Subscribers concurrently,
// channels only allow for one Consumer to read a given message.

// creates a channel with no capacity limits
// we might run into memory constraints if producer produces large amount of data.
var unboundedChannel = Channel.CreateUnbounded<string>();

var producer = new Producer(unboundedChannel.Writer);
var consumer = new Consumer(unboundedChannel.Reader);

// starting producer on a different thread.
_ = Task.Factory.StartNew(async () =>
{
    await producer.ProduceTodoAsync();
});

await consumer.ConsumeWorkAsync();