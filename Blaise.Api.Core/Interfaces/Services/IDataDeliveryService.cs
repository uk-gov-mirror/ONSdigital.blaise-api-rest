using System.Threading.Tasks;
using Blaise.Api.Contracts.Models.Instrument;

namespace Blaise.Api.Core.Interfaces.Services
{
    public interface IDataDeliveryService
    {
        Task DeliverInstrumentAsync(string serverParkName, InstrumentPackageDto instrumentPackageDto);
    }
}