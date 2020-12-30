using System;
using System.IO;
using System.IO.Abstractions;
using Blaise.Api.Storage.Interfaces;
using Google.Cloud.Storage.V1;

namespace Blaise.Api.Storage.Providers
{
    public class CloudStorageClientProvider : ICloudStorageClientProvider, IDisposable
    {
        private readonly IFileSystem _fileSystem;

        private StorageClient _storageClient;

        public CloudStorageClientProvider(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public void Download(string bucketName, string fileName, string destinationFilePath)
        {
            var storageClient = GetStorageClient();
            using (var fileStream = _fileSystem.FileStream.Create(destinationFilePath, FileMode.OpenOrCreate))
            {
                storageClient.DownloadObject(bucketName, fileName, fileStream);
            }
        }
        
        public void Dispose()
        {
            _storageClient?.Dispose();
            _storageClient = null;
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
