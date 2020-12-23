namespace Blaise.Api.Storage.Interfaces
{
    public interface ICloudStorageClientProvider
    {
        void Download(string bucketPath, string fileName, string destinationFilePath);
        void Dispose();
    }
}