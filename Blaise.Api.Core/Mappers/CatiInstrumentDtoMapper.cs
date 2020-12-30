using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Api.Contracts.Models.Instrument;
using Blaise.Api.Core.Interfaces.Mappers;

namespace Blaise.Api.Core.Mappers
{
    public class CatiInstrumentDtoMapper : ICatiInstrumentDtoMapper
    {
        public CatiInstrumentDto MapToCatiInstrumentDto(InstrumentDto instrumentDto, List<DateTime> surveyDays)
        {
            var catiInstrument = new CatiInstrumentDto
            {
                Name = instrumentDto.Name,
                ServerParkName = instrumentDto.ServerParkName,
                InstallDate = instrumentDto.InstallDate,
                Status = instrumentDto.Status,
                SurveyDays = surveyDays,
                Expired = surveyDays.All(s => s.Date < DateTime.Today),
                ActiveToday = surveyDays.Any(s => s.Date == DateTime.Today)
            };

            return catiInstrument;
        }

    }
}
