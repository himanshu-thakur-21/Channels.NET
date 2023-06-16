namespace Channels.NET.Bounded.ChannelData
{
    public class Envelope
    {
        public string Payload { get; }

        public Envelope(string payload)
        {
            Payload = payload;
        }
    }
}
