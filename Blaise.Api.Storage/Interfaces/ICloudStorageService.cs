using System.Threading.Tasks;

namespace Blaise.Api.Storage.Interfaces
{
    public interface ICloudStorageService
    {
        Task<string> DownloadPackageFromInstrumentBucketAsync(string fileName, string tempFilePath);
        Task DownloadDatabaseFilesFromNisraBucketAsync(string bucketPath, string tempFilePath);
    }
}