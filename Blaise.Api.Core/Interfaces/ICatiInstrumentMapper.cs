using System;
using System.Collections.Generic;
using Blaise.Api.Contracts.Models;
using Blaise.Api.Contracts.Models.Instrument;

namespace Blaise.Api.Core.Interfaces
{
    public interface ICatiInstrumentDtoMapper
    {
        CatiInstrumentDto MapToCatiInstrumentDto(InstrumentDto instrumentDto, List<DateTime> surveyDays);
    }
}