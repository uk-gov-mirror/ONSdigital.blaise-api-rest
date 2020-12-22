using System.Collections.Generic;
using Blaise.Api.Contracts.Models;

namespace Blaise.Api.Core.Interfaces
{
    public interface IServerParkService
    {
        IEnumerable<ServerParkDto> GetServerParks();

        ServerParkDto GetServerPark(string serverParkName);

        bool ServerParkExists(string serverParkName);

        void RegisterMachineOnServerPark(string serverParkName, string machineName);
    }
}