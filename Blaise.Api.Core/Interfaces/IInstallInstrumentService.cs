namespace Blaise.Api.Core.Interfaces
{
    public interface IInstallInstrumentService
    {
        void InstallInstrument(string bucketPath, string instrumentFileName, string serverParkName);

        void UninstallInstrument(string instrumentName, string serverParkName);
    }
}