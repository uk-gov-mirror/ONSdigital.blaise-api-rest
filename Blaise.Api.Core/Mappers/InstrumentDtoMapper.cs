using System.Collections.Generic;
using Blaise.Api.Contracts.Models.Instrument;
using Blaise.Api.Core.Interfaces.Mappers;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Api.Core.Mappers
{
    public class InstrumentDtoMapper : IInstrumentDtoMapper
    {
        public IEnumerable<InstrumentDto> MapToInstrumentDtos(IEnumerable<ISurvey> instruments)
        {
            var instrumentDtoList = new List<InstrumentDto>();

            foreach (var instrument in instruments)
            {
                instrumentDtoList.Add(MapToInstrumentDto(instrument));
            }

            return instrumentDtoList;
        }

        public InstrumentDto MapToInstrumentDto(ISurvey instrument)
        {
            return new InstrumentDto
            {
                Name = instrument.Name,
                ServerParkName = instrument.ServerPark,
                InstallDate = instrument.InstallDate,
                Status = instrument.Status,
                DataRecordCount = GetNumberOfDataRecords(instrument as ISurvey2)
            };
        }

        private static int GetNumberOfDataRecords(ISurvey2 instrument)
        {
            var reportingInfo = instrument.GetReportingInfo();

            return reportingInfo.DataRecordCount;
        }
    }
}
