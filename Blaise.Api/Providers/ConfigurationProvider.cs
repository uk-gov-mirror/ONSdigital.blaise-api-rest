using System;
using System.Configuration;
using Blaise.Api.Contracts.Interfaces;

namespace Blaise.Api.Providers
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        public static string BaseUrl => Environment.GetEnvironmentVariable("ENV_BASE_URL", EnvironmentVariableTarget.Machine)
                                     ?? ConfigurationManager.AppSettings["ENV_BASE_URL"];

        public string TempDownloadPath => Environment.GetEnvironmentVariable("ENV_TEMP_DOWNLOAD_PATH", EnvironmentVariableTarget.Machine)
                                                ?? ConfigurationManager.AppSettings["ENV_TEMP_DOWNLOAD_PATH"];
    }
}
