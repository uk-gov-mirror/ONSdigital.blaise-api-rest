using System;

namespace Blaise.Api.Log.Services
{
    public class LogService 
    {
        public static void Info(string message)
        {
            Console.WriteLine(message);
        }

        public static void Error(string message, Exception exception)
        {
            Console.WriteLine($"{message}: {exception.Message}, {exception.InnerException}");
        }
    }
}
