using System;

namespace Blaise.Api.Logging.Services
{
    public class LoggingService 
    {
        public static void LogInfo(string message)
        {
            Console.WriteLine(message);
        }

        public static void LogError(string message, Exception exception)
        {
            Console.WriteLine($"{message}: {exception.Message}, {exception.InnerException}");
        }
    }
}
