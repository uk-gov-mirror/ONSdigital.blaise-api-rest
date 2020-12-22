using System;
using System.Configuration;
using Blaise.Api.Contracts.Interfaces;

namespace Blaise.Api.Providers
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        public string TempDownloadPath => Environment.GetEnvironmentVariable("ENV_TEMP_DOWNLOAD_PATH", EnvironmentVariableTarget.Machine)
                                                ?? ConfigurationManager.AppSettings["TempDownloadPath"];
    }
}
