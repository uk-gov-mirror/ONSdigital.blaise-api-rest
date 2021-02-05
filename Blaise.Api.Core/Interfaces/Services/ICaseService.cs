namespace Blaise.Api.Core.Interfaces.Services
{
    public interface ICaseService
    {
        void ImportOnlineDatabaseFile(string databaseFilePath, string instrumentName, string serverParkName);
    }
}