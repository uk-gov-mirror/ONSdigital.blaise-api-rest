using System;
using System.Configuration;

namespace Blaise.Api.Configuration
{
    public class PortConfig
    {
        public static string BaseUrl => Environment.GetEnvironmentVariable("ENV_BASE_URL", EnvironmentVariableTarget.Machine)
                                 ?? ConfigurationManager.AppSettings["BaseUrl"];
    }
}
