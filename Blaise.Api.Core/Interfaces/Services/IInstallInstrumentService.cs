using Blaise.Api.Contracts.Models.Instrument;

namespace Blaise.Api.Core.Interfaces.Services
{
    public interface IInstallInstrumentService
    {
        void InstallInstrument(string serverParkName, InstallInstrumentDto installInstrumentDto);

        void UninstallInstrument(string instrumentName, string serverParkName);
    }
}