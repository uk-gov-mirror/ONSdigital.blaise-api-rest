using System.Threading.Tasks;

namespace Blaise.Api.Storage.Interfaces
{
    public interface ICloudStorageService
    {
        Task<string> DownloadPackageFromInstrumentBucketAsync(string fileName);
        Task<string> DownloadDatabaseFilesFromNisraBucketAsync(string bucketPath);
    }
}