using System.Collections.Generic;
using Blaise.Api.Contracts.Models.Instrument;

namespace Blaise.Api.Contracts.Models.ServerPark
{
    public class ServerParkDto
    {
        public ServerParkDto()
        {
            Instruments = new List<InstrumentDto>();
        }

        public string Name { get; set; }

        public IEnumerable<InstrumentDto> Instruments { get; set; }

        public IEnumerable<MachineDto> Servers { get; set; }
    }
}
