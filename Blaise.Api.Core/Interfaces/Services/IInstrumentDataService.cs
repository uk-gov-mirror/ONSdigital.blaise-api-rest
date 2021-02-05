using System.Threading.Tasks;

namespace Blaise.Api.Core.Interfaces.Services
{
    public interface IInstrumentDataService
    {
        Task<string> GetInstrumentPackageWithDataAsync(string serverParkName, string instrumentName);

        Task ImportOnlineDataAsync(string bucketPath, string serverParkName, string instrumentName);
    }
}