﻿using System.Threading.Tasks;

namespace Blaise.Api.Storage.Interfaces
{
    public interface ICloudStorageService
    {
        Task<string> DownloadFromBucketAsync(string bucketPath, string fileName, string localFileName);
        Task UploadToBucketAsync(string bucketPath, string filePath);
    }
}