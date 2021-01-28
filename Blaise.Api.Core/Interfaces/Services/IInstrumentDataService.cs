using System.Threading.Tasks;
using Blaise.Api.Contracts.Models.Instrument;

namespace Blaise.Api.Core.Interfaces.Services
{
    public interface IInstrumentDataService
    {
        Task<string> CreateInstrumentPackageWithDataAsync(string serverParkName, InstrumentPackageDto instrumentPackageDto);
    }
}