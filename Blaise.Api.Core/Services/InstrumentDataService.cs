using System.Threading.Tasks;
using Blaise.Api.Contracts.Interfaces;
using Blaise.Api.Contracts.Models.Instrument;
using Blaise.Api.Core.Extensions;
using Blaise.Api.Core.Interfaces.Services;
using Blaise.Api.Storage.Interfaces;

namespace Blaise.Api.Core.Services
{
    public class InstrumentDataService : IInstrumentDataService
    {
        private readonly IFileService _fileService;
        private readonly ICaseService _caseService;
        private readonly ICloudStorageService _storageService;
        private readonly ILoggingService _loggingService;

        public InstrumentDataService(
            IFileService fileService,
            ICaseService caseService,
            ICloudStorageService storageService, 
            ILoggingService loggingService)
        {
            _fileService = fileService;
            _caseService = caseService;
            _storageService = storageService;
            _loggingService = loggingService;
        }

        public async Task<string> GetInstrumentPackageWithDataAsync(string serverParkName, string instrumentName)
        {
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");

            return await CreateInstrumentPackageWithDataAsync(serverParkName, instrumentName);
        }

        public async Task ImportOnlineDataAsync(InstrumentDataDto instrumentDataDto, string serverParkName,
            string instrumentName)
        {
            instrumentDataDto.ThrowExceptionIfNull("InstrumentDataDto");
            instrumentDataDto.InstrumentDataPath.ThrowExceptionIfNullOrEmpty("instrumentDataDto.InstrumentDataPath");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");

            var filePath = await DownloadDatabaseFilesFromBucketAsync(instrumentDataDto.InstrumentDataPath);
            var databaseFile = _fileService.GetDatabaseFile(filePath, instrumentName);

            _caseService.ImportOnlineDatabaseFile(databaseFile, instrumentName, serverParkName);
            _fileService.DeletePath(filePath);
        }

        private async Task<string> CreateInstrumentPackageWithDataAsync(string serverParkName, string instrumentName)
        {
            var instrumentPackage = await DownloadInstrumentFromBucketAsync(instrumentName);
            _loggingService.LogInfo($"Downloaded instrument package '{instrumentPackage}'");
            
            _fileService.UpdateInstrumentFileWithData(serverParkName, instrumentPackage);
            _loggingService.LogInfo($"Updated instrument package '{instrumentPackage}' with data");

            return instrumentPackage;
        }

        private async Task<string> DownloadInstrumentFromBucketAsync(string instrumentName)
        {
            var instrumentPackageName = _fileService.GetInstrumentPackageName(instrumentName);

            _loggingService.LogInfo($"Downloading instrument package '{instrumentPackageName}'");
            return await _storageService.DownloadPackageFromInstrumentBucketAsync(instrumentPackageName);
        }

        private async Task<string> DownloadDatabaseFilesFromBucketAsync(string bucketPath)
        {
            _loggingService.LogInfo($"Downloading instrument files from nisra bucket path '{bucketPath}'");
            return await _storageService.DownloadDatabaseFilesFromNisraBucketAsync(bucketPath);
        }
    }
}
