using System.Collections.Generic;
using Blaise.Api.Contracts.Models;
using Blaise.Api.Contracts.Models.Instrument;

namespace Blaise.Api.Core.Interfaces
{
    public interface IInstrumentService
    {
        IEnumerable<InstrumentDto> GetAllInstruments();

        IEnumerable<InstrumentDto> GetInstruments(string serverParkName);

        InstrumentDto GetInstrument(string instrumentName, string serverParkName);

        bool InstrumentExists(string instrumentName, string serverParkName);
    }
}
