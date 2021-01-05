using System.Collections.Generic;
using Blaise.Api.Contracts.Models.Cati;

namespace Blaise.Api.Core.Interfaces.Services
{
    public interface ICatiService
    {
        List<CatiInstrumentDto> GetCatiInstruments();

        void CreateDayBatch(string instrumentName, string serverParkName, DayBatchDto dayBatchDate);
    }
}