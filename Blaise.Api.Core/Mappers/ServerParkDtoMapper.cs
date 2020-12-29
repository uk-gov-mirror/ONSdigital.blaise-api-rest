using System.Collections.Generic;
using Blaise.Api.Contracts.Models;
using Blaise.Api.Contracts.Models.ServerPark;
using Blaise.Api.Core.Interfaces.Mappers;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Api.Core.Mappers
{
    public class ServerParkDtoMapper : IServerParkDtoMapper
    {
        private readonly IInstrumentDtoMapper _mapper;

        public ServerParkDtoMapper(IInstrumentDtoMapper mapper)
        {
            _mapper = mapper;
        }

        public IEnumerable<ServerParkDto> MapToServerParkDtos(IEnumerable<IServerPark> serverParks)
        {
            var serverParkDtoList = new List<ServerParkDto>();

            foreach (var serverPark in serverParks)
            {
                serverParkDtoList.Add(MapToServerParkDto(serverPark));
            }

            return serverParkDtoList;
        }

        public ServerParkDto MapToServerParkDto(IServerPark serverPark)
        {
            return new ServerParkDto
            {
                Name = serverPark.Name,
                Instruments = _mapper.MapToInstrumentDtos(serverPark.Surveys)
            };
        }
    }
}
