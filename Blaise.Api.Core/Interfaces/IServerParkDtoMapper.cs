﻿using System.Collections.Generic;
using Blaise.Api.Contracts.Models;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Api.Core.Interfaces
{
    public interface IServerParkDtoMapper
    {
        IEnumerable<ServerParkDto> MapToServerParkDtos(IEnumerable<IServerPark> serverParks);
        ServerParkDto MapToServerParkDto(IServerPark serverPark);
    }
}