namespace Blaise.Api.Core.Interfaces.Services
{
    public interface IUninstallInstrumentService
    {
        void UninstallInstrument(string instrumentName, string serverParkName);
    }
}