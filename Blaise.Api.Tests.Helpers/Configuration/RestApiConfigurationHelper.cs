using Blaise.Api.Tests.Helpers.Extensions;

namespace Blaise.Api.Tests.Helpers.Configuration
{
    public static class RestApiConfigurationHelper
    {
        public static string BaseUrl => ConfigurationExtensions.GetVariable("RestApiBaseUrl");

        public static string InstrumentsUrl =>
            ConfigurationExtensions.GetVariable(
                $"/api/v1/serverparks/{BlaiseConfigurationHelper.ServerParkName}/instruments");
    }
}
