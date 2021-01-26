namespace Blaise.Api.Core.Interfaces.Services
{
    public interface IInstrumentUninstallerService
    {
        void UninstallInstrument(string instrumentName, string serverParkName);
    }
}