using System;
using System.IO.Abstractions;
using Blaise.Api.Contracts.Interfaces;
using Blaise.Api.Storage.Interfaces;

namespace Blaise.Api.Storage.Services
{
    public class StorageService : IStorageService
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly ICloudStorageClientProvider _cloudStorageClient;
        private readonly IFileSystem _fileSystem;

        public StorageService(
            IConfigurationProvider configurationProvider, 
            ICloudStorageClientProvider cloudStorageClient,
            IFileSystem fileSystem)
        {
            _configurationProvider = configurationProvider;
            _cloudStorageClient = cloudStorageClient;
            _fileSystem = fileSystem;
        }

        public string DownloadFromBucket(string bucketPath, string fileName)
        {
            var destinationFilePath = TemporaryDownloadPath();
            _cloudStorageClient.Download(bucketPath, fileName, destinationFilePath);
            _cloudStorageClient.Dispose();

            return _fileSystem.Path.Combine(destinationFilePath, fileName);
        }

        public void DeleteFile(string instrumentFile)
        {
            _fileSystem.File.Delete(instrumentFile);
        }

        private string TemporaryDownloadPath()
        {
            return _fileSystem.Path.Combine(_configurationProvider.TempDownloadPath, Guid.NewGuid().ToString());
        }
    }
}
