using System.Threading;
using Microsoft.Owin.Hosting;

namespace Blaise.Api
{
    internal class Program
    {
        private static void Main()
        {
            var baseAddress = "http://*:80/";
            //var baseAddress = "http://localhost:5000/";

            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                Thread.Sleep(Timeout.Infinite);
            }
        }
    }
}
