using System;
using System.ServiceProcess;
using System.Threading;
using Blaise.Api.Providers;
using Microsoft.Owin.Hosting;

namespace Blaise.Api
{
    internal class Program
    {
        static void Main()
        {
            var servicesToRun = new ServiceBase[]
            {
                new ApiService()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
