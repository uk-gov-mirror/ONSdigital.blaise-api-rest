using System;
using System.IO.Abstractions;
using System.Threading.Tasks;
using Blaise.Api.Contracts.Interfaces;
using Blaise.Api.Storage.Interfaces;

namespace Blaise.Api.Storage.Services
{
    public class CloudStorageService : ICloudStorageService
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly ICloudStorageClientProvider _cloudStorageClient;
        private readonly IFileSystem _fileSystem;

        public CloudStorageService(
            IConfigurationProvider configurationProvider,
            ICloudStorageClientProvider cloudStorageClient,
            IFileSystem fileSystem)
        {
            _configurationProvider = configurationProvider;
            _cloudStorageClient = cloudStorageClient;
            _fileSystem = fileSystem;
        }

        public async Task<string> DownloadPackageFromInstrumentBucketAsync(string fileName)
        {
            var localFilePath = _fileSystem.Path.Combine(
                _configurationProvider.TempPath,
                "InstrumentPackages",
                Guid.NewGuid().ToString());

            return await DownloadFromBucketAsync(_configurationProvider.DqsBucket, fileName, localFilePath);
        }

        public async Task<string> DownloadDatabaseFilesFromNisraBucketAsync(string bucketPath)
        {
            var localFilePath = _fileSystem.Path.Combine(
                _configurationProvider.TempPath,
                "InstrumentFiles",
                Guid.NewGuid().ToString());

            var files = await _cloudStorageClient.GetListOfFiles(_configurationProvider.NisraBucket, bucketPath);

            foreach (var file in files)
            {
                await DownloadFromBucketAsync(_configurationProvider.NisraBucket, file, localFilePath);
            }

            return localFilePath;
        }

        public async Task<string> DownloadFromBucketAsync(string bucketName, string fileName, string localFilePath)
        {
            if (!_fileSystem.Directory.Exists(localFilePath))
            {
                _fileSystem.Directory.CreateDirectory(localFilePath);
            }

            var downloadedFile = _fileSystem.Path.Combine(localFilePath, fileName);
            await _cloudStorageClient.DownloadAsync(bucketName, fileName, downloadedFile);

            return downloadedFile;
        }
    }
}