using System.Threading.Tasks;

namespace Blaise.Api.Storage.Interfaces
{
    public interface IStorageService
    {
        Task<string> DownloadFromBucketAsync(string bucketPath, string fileName);
        Task UploadToBucketAsync(string bucketPath, string fileName);
        void DeleteFile(string instrumentFile);
    }
}