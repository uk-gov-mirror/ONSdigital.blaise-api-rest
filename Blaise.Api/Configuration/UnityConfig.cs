using Blaise.Api.Core.Interfaces;
using Blaise.Api.Core.Mappers;
using Blaise.Api.Core.Services;
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Core.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Core.Services;
using Blaise.Nuget.Api.Providers;
using Unity;
using Unity.Injection;

namespace Blaise.Api.Configuration
{
    public static class UnityConfig
    {
        public static IUnityContainer GetConfiguredContainer()
        {
			var container = new UnityContainer();

            var configurationProvider = new ConfigurationProvider();
            var connectionModel = configurationProvider.GetConnectionModel();
            
            //blaise api
            container.RegisterType<IBlaiseServerParkApi, BlaiseServerParkApi>(new InjectionConstructor(connectionModel));
            container.RegisterType<IBlaiseSurveyApi, BlaiseSurveyApi>(new InjectionConstructor(connectionModel));
            container.RegisterType<IBlaiseCatiApi, BlaiseCatiApi>(new InjectionConstructor(connectionModel));

            //core mappers
            container.RegisterType<IServerParkDtoMapper, ServerParkDtoMapper>();
            container.RegisterType<IInstrumentDtoMapper, InstrumentDtoMapper>();
            container.RegisterType<ICatiInstrumentMapper, CatiInstrumentMapper>();

            //core services
            container.RegisterType<IServerParkService, ServerParkService>();
            container.RegisterType<IInstrumentService, InstrumentService>();
            container.RegisterType<ICatiService, CatiService>();
            container.RegisterType<IHealthCheckService, HealthCheckService>();

            //factory services for health check
            container.RegisterType<IPasswordService, PasswordService>();
            container.RegisterType<IConfigurationProvider, ConfigurationProvider>();
            container.RegisterType<IConnectedServerFactory, ConnectedServerFactory>();
            container.RegisterType<IRemoteDataServerFactory, RemoteDataServerFactory>();
            container.RegisterType<ICatiManagementServerFactory, CatiManagementServerFactory>();

            return container;
        }
    }
}