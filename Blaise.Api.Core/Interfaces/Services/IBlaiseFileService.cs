namespace Blaise.Api.Core.Interfaces.Services
{
    public interface IBlaiseFileService
    {
        void UpdateInstrumentFileWithSqlConnection(string instrumentName, string instrumentFile);
        void UpdateInstrumentFileWithData(string serverParkName, string instrumentName, string instrumentFile);
        void DeleteFile(string instrumentFile);
        string GenerateUniqueInstrumentFile(string instrumentFile, string instrumentName);
    }
}