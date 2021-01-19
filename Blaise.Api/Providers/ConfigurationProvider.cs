using System;
using System.Configuration;
using Blaise.Api.Contracts.Interfaces;

namespace Blaise.Api.Providers
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        public string BaseUrl => ConfigurationManager.AppSettings["BASE_URL"];

        public string TempPath => ConfigurationManager.AppSettings["TEMP_PATH"];
    }
}
