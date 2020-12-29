using System;
using System.Collections.Generic;
using Blaise.Api.Contracts.Models.Instrument;

namespace Blaise.Api.Core.Interfaces.Services
{
    public interface IInstrumentService
    {
        IEnumerable<InstrumentDto> GetAllInstruments();

        IEnumerable<InstrumentDto> GetInstruments(string serverParkName);

        InstrumentDto GetInstrument(string instrumentName, string serverParkName);

        bool InstrumentExists(string instrumentName, string serverParkName);

        Guid GetInstrumentId(string instrumentName, string serverParkName);
    }
}
