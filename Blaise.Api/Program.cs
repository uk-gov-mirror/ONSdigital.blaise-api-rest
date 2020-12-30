using System.ServiceProcess;

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
