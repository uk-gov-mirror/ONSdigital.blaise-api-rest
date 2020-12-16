using Blaise.Api.Tests.Helpers.Extensions;
using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Api.Tests.Helpers.Configuration
{
    public static class BlaiseConfigurationHelper
    {
        public static string ServerParkName => ConfigurationExtensions.GetEnvironmentVariable("ServerParkName");
        public static string InstrumentPath => ConfigurationExtensions.GetEnvironmentVariable("InstrumentPath");
        public static string InstrumentName => ConfigurationExtensions.GetEnvironmentVariable("InstrumentName");
        public static string InstrumentPackage => $"{InstrumentPath}//{InstrumentName}.zip";
    }
}
