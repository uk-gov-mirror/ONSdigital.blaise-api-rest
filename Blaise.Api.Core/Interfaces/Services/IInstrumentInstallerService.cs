using System.Threading.Tasks;
using Blaise.Api.Contracts.Models.Instrument;

namespace Blaise.Api.Core.Interfaces.Services
{
    public interface IInstrumentInstallerService
    {
        Task InstallInstrumentAsync(string serverParkName, InstrumentPackageDto instrumentPackageDto);
    }
}