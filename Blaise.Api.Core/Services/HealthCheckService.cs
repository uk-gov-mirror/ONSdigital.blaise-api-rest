using System.Collections.Generic;
using Blaise.Api.Contracts.Enums;
using Blaise.Api.Contracts.Models;
using Blaise.Api.Core.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Providers;

namespace Blaise.Api.Core.Services
{
    public class HealthCheckService : IHealthCheckService
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IConnectedServerFactory _connectedServerFactory;
        private readonly IRemoteDataServerFactory _remoteDataServerFactory;
        private readonly ICatiManagementServerFactory _catiManagementServerFactory;

        public HealthCheckService(
            IConfigurationProvider configurationProvider,
            IConnectedServerFactory connectedServerFactory,
            IRemoteDataServerFactory remoteDataServerFactory,
            ICatiManagementServerFactory catiManagementServerFactory)
        {
            _configurationProvider = configurationProvider;
            _connectedServerFactory = connectedServerFactory;
            _remoteDataServerFactory = remoteDataServerFactory;
            _catiManagementServerFactory = catiManagementServerFactory;
        }

        public IEnumerable<HealthCheckResultDto> PerformCheck()
        {
            var connectionModel = _configurationProvider.GetConnectionModel();

            return new List<HealthCheckResultDto>
            {
                CheckConnectionModel(connectionModel),
                CheckConnection(connectionModel),
                CheckRemoteDataServerConnection(connectionModel),
                CheckRemoteCatiManagementConnection(connectionModel)
            };
        }

        private static HealthCheckResultDto CheckConnectionModel(ConnectionModel connectionModel)
        {
            if (!string.IsNullOrWhiteSpace(connectionModel.ServerName) &&
                !string.IsNullOrWhiteSpace(connectionModel.UserName) &&
                !string.IsNullOrWhiteSpace(connectionModel.Password) &&
                !string.IsNullOrWhiteSpace(connectionModel.Binding) &&
                connectionModel.Port > 0 &&
                connectionModel.RemotePort > 0)
            {
                return new HealthCheckResultDto(HealthCheckType.ConnectionModel, HealthStatusType.Ok);
            }

            return new HealthCheckResultDto(HealthCheckType.ConnectionModel, HealthStatusType.NotOk);
        }

        private HealthCheckResultDto CheckConnection(ConnectionModel connectionModel)
        {
            try
            {
                _connectedServerFactory.GetConnection(connectionModel);
                return new HealthCheckResultDto(HealthCheckType.Connection, HealthStatusType.Ok);
            }
            catch
            {
                return new HealthCheckResultDto(HealthCheckType.Connection, HealthStatusType.NotOk);
            }
        }

        private HealthCheckResultDto CheckRemoteDataServerConnection(ConnectionModel connectionModel)
        {
            try
            {
                _remoteDataServerFactory.GetConnection(connectionModel);
                return new HealthCheckResultDto(HealthCheckType.RemoteDataServer, HealthStatusType.Ok);
            }
            catch
            {
                return new HealthCheckResultDto(HealthCheckType.RemoteDataServer, HealthStatusType.NotOk);
            }
        }

        private HealthCheckResultDto CheckRemoteCatiManagementConnection(ConnectionModel connectionModel)
        {
            try
            {
                _catiManagementServerFactory.GetConnection(connectionModel);
                return new HealthCheckResultDto(HealthCheckType.RemoteCatiManagement, HealthStatusType.Ok);
            }
            catch
            {
                return new HealthCheckResultDto(HealthCheckType.RemoteCatiManagement, HealthStatusType.NotOk);
            }
        }
    }
}
