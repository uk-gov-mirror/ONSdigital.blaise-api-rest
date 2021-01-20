using System.Threading.Tasks;

namespace Blaise.Api.Storage.Interfaces
{
    public interface ICloudStorageClientProvider
    {
        Task DownloadAsync(string bucketPath, string fileName, string destinationFilePath);
        void Dispose();
    }
}