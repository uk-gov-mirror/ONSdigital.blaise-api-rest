using Blaise.Api.Core.Interfaces;
using Blaise.Api.Core.Services;
using Blaise.Nuget.Api;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Unity;

namespace Blaise.Api.Framework
{
    public static class UnityConfig
    {
        public static IUnityContainer GetConfiguredContainer()
        {
			var container = new UnityContainer();

            container.RegisterType<IFluentBlaiseApi, FluentBlaiseApi>();
            container.RegisterType<IParkService, ParkService>();

            return container;
        }
    }
}