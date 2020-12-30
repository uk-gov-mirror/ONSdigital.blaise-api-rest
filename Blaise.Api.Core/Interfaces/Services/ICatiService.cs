using System.Collections.Generic;
using Blaise.Api.Contracts.Models.Instrument;

namespace Blaise.Api.Core.Interfaces.Services
{
    public interface ICatiService
    {
        List<CatiInstrumentDto> GetCatiInstruments();
    }
}