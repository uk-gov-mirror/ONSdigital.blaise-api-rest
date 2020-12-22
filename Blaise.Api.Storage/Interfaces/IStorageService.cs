namespace Blaise.Api.Storage.Interfaces
{
    public interface IStorageService
    {
        string DownloadFromBucket(string bucketPath, string fileName);
        void DeleteFile(string instrumentFile);
    }
}