namespace Blaise.Api.Core.Interfaces.Services
{
    public interface ICaseService
    {
        int GetNumberOfCases(string instrumentName, string serverParkName);
    }
}