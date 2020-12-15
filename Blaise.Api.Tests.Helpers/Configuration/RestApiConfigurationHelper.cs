using Blaise.Api.Tests.Behaviour.Extensions;

namespace Blaise.Api.Tests.Helpers.Configuration
{
    public static class RestApiConfigurationHelper
    {
        public static string BaseUrl => ConfigurationExtensions.GetVariable("RestApiBaseUrl");
    }
}
