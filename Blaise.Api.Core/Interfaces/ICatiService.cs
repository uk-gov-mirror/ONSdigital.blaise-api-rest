using System.Collections.Generic;
using Blaise.Api.Contracts.Models;

namespace Blaise.Api.Core.Interfaces
{
    public interface ICatiService
    {
        List<CatiInstrumentDto> GetCatiInstruments();
    }
}