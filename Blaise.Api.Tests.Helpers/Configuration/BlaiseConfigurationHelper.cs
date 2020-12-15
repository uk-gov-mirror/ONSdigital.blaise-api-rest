using Blaise.Api.Tests.Behaviour.Extensions;
using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Api.Tests.Helpers.Configuration
{
    public static class BlaiseConfigurationHelper
    {
        public static string ServerParkName => ConfigurationExtensions.GetEnvironmentVariable("ServerParkName");
        public static string InstrumentPath => ConfigurationExtensions.GetEnvironmentVariable("InstrumentPath");
        public static string InstrumentName => ConfigurationExtensions.GetEnvironmentVariable("InstrumentName");
        public static string InstrumentPackage => $"{InstrumentPath}//{InstrumentName}.zip";

        public static ConnectionModel BuildConnectionModel()
        {
            return new ConnectionModel
            {
                ServerName = ConfigurationExtensions.GetEnvironmentVariable("BlaiseServerHostName"),
                UserName = ConfigurationExtensions.GetEnvironmentVariable("BlaiseServerUserName"),
                Password = ConfigurationExtensions.GetEnvironmentVariable("BlaiseServerPassword"),
                Binding = ConfigurationExtensions.GetEnvironmentVariable("BlaiseServerBinding"),
                Port = ConfigurationExtensions.GetIntEnvironmentVariable("BlaiseConnectionPort"),
                RemotePort = ConfigurationExtensions.GetIntEnvironmentVariable("BlaiseRemoteConnectionPort"),
                ConnectionExpiresInMinutes = ConfigurationExtensions.GetIntEnvironmentVariable("ConnectionExpiresInMinutes")
            };
        }
    }
}
