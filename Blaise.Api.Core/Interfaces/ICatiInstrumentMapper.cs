using System;
using System.Collections.Generic;
using Blaise.Api.Contracts.Models;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Api.Core.Interfaces
{
    public interface ICatiInstrumentMapper
    {
        CatiInstrumentDto MapToDto(ISurvey instrument, List<DateTime> surveyDays);
    }
}