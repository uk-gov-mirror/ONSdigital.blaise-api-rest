using System;
using System.Collections.Generic;
using Blaise.Api.Contracts.Models;

namespace Blaise.Api.Core.Interfaces
{
    public interface ICatiInstrumentDtoMapper
    {
        CatiInstrumentDto MapToCatiInstrumentDto(InstrumentDto instrumentDto, List<DateTime> surveyDays);
    }
}