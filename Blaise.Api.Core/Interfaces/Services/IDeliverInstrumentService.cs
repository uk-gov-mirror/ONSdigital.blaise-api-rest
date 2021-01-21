using System.Threading.Tasks;
using Blaise.Api.Contracts.Models.Instrument;

namespace Blaise.Api.Core.Interfaces.Services
{
    public interface IDeliverInstrumentService
    {
        Task DeliverInstrumentWithDataAsync(string serverParkName, InstrumentPackageDto instrumentPackageDto);
    }
}