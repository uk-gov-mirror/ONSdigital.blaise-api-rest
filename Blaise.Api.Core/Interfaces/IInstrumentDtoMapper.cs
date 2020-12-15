﻿using System.Collections.Generic;
using Blaise.Api.Contracts.Models;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Api.Core.Interfaces
{
    public interface IInstrumentDtoMapper
    {
        IEnumerable<InstrumentDto> MapToInstrumentDtos(IEnumerable<ISurvey> instruments);
        InstrumentDto MapToInstrumentDto(ISurvey instrument);
    }
}