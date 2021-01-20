namespace Blaise.Api.Core.Interfaces.Services
{
    public interface IFileService
    {
        void UpdateInstrumentFileWithSqlConnection(string instrumentName, string instrumentFile);
    }
}