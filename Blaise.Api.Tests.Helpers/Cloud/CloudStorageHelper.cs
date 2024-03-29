﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blaise.Api.Tests.Helpers.Configuration;
using Google.Cloud.Storage.V1;

namespace Blaise.Api.Tests.Helpers.Cloud
{
    public class CloudStorageHelper
    {
        private StorageClient _storageClient;
        private static CloudStorageHelper _currentInstance;

        public static CloudStorageHelper GetInstance()
        {
            return _currentInstance ?? (_currentInstance = new CloudStorageHelper());
        }

        public async Task UploadToBucketAsync(string bucketPath, string filePath)
        {
            var storageClient = GetStorageClient();
            using (var fileStream = File.OpenRead(filePath))
            {
                await storageClient.UploadObjectAsync(bucketPath, Path.GetFileName(filePath), null, fileStream);
            }
        }

        public async Task UploadFolderToBucketAsync(string bucketPath, string folderPath)
        {
            var storageClient = GetStorageClient();
            var filesInFolder = Directory.GetFiles(folderPath);
            foreach (var file in filesInFolder)
            {
                using (var fileStream = File.OpenRead(file))
                {
                    await storageClient.UploadObjectAsync(bucketPath, $"{BlaiseConfigurationHelper.InstrumentName}/{Path.GetFileName(file)}", null, fileStream);
                }
            }          
        }

        public async Task<string> DownloadFromBucketAsync(string bucketPath, string fileName, string destinationFilePath)
        {
            var storageClient = GetStorageClient();

            if (!Directory.Exists(destinationFilePath))
            {
                Directory.CreateDirectory(destinationFilePath);
            }

            var downloadedFile = Path.Combine(destinationFilePath, fileName);

            using (var fileStream = File.OpenWrite(downloadedFile))
            {
                await storageClient.DownloadObjectAsync(bucketPath, fileName, fileStream);
            }

            return downloadedFile;
        }

        public async Task DeleteFileInBucketAsync(string bucketPath, string fileName)
        {
            var storageClient = GetStorageClient();
            await storageClient.DeleteObjectAsync(bucketPath, fileName);
        }

        public async Task DeleteFolderInBucketAsync(string bucketPath, string fileName)
        {
            var storageClient = GetStorageClient();
            await storageClient.DeleteObjectAsync(bucketPath, $"{fileName}/");
        }

        public async Task DeleteFilesInBucketAsync(string bucketName, string bucketPath)
        {
            var storageClient = GetStorageClient();
            var storageObjects = storageClient.ListObjects(bucketName, $"{bucketPath}/");

            foreach (var storageObject in storageObjects)
            {
                await storageClient.DeleteObjectAsync(bucketName, storageObject.Name);
            }
        }

        private StorageClient GetStorageClient()
        {
            var client = _storageClient;

            if (client != null)
            {
                return client;
            }

            return _storageClient = StorageClient.Create();
        }
    }
}
