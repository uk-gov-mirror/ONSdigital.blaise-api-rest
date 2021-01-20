using System.IO.Abstractions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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

        public async Task<string> DownloadFromBucketAsync(string bucketPath, string fileName)
        {
            var destinationFilePath = _fileSystem.Path.Combine(_configurationProvider.TempPath, fileName);
            await _cloudStorageClient.DownloadAsync(bucketPath, fileName, destinationFilePath);
            _cloudStorageClient.Dispose();

            return destinationFilePath;
        }

        public async Task UploadToBucketAsync(string bucketPath, string fileName)
        {
            _cloudStorageClient.Dispose();
        }

        public void DeleteFile(string instrumentFile)
        {
            _fileSystem.File.Delete(instrumentFile);
        }
    }
}
