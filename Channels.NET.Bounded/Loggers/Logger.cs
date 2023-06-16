namespace Channels.NET.Bounded.Loggers
{
    public class Logger
    {
        public static void Log(string message, ConsoleColor consoleColor = ConsoleColor.White)
        {
            // since this method might be called by different threads,
            // we need to use a lock to guarantee we set the right color
            lock (Console.Out)
            {
                Console.ForegroundColor = consoleColor;
                Console.WriteLine($"[{DateTime.UtcNow:hh:mm:ss.ff}] - {message}");
            }
        }
    }
}
