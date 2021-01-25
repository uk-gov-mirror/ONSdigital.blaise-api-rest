using System.Threading.Tasks;
using Blaise.Api.Contracts.Models.Instrument;
using Blaise.Api.Core.Extensions;
using Blaise.Api.Core.Interfaces.Services;
using Blaise.Api.Storage.Interfaces;

namespace Blaise.Api.Core.Services
{
    public class InstrumentDataService : IInstrumentDataService
    {
        private readonly IBlaiseFileService _fileService;
        private readonly ICloudStorageService _storageService;

        public InstrumentDataService(
            IBlaiseFileService fileService,
            ICloudStorageService storageService)
        {
            _fileService = fileService;
            _storageService = storageService;
        }

        public async Task<string> CreateInstrumentPackageWithDataAsync(string serverParkName,
            InstrumentPackageDto instrumentPackageDto)
        {
            instrumentPackageDto.InstrumentName.ThrowExceptionIfNullOrEmpty("instrumentPackageDto.InstrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            instrumentPackageDto.BucketPath.ThrowExceptionIfNullOrEmpty("instrumentPackageDto.BucketPath");
            instrumentPackageDto.InstrumentFile.ThrowExceptionIfNullOrEmpty("instrumentPackageDto.InstrumentFile");

            var instrumentPackage = await DownloadInstrumentAsync(instrumentPackageDto);
      
            _fileService.UpdateInstrumentFileWithData(serverParkName, instrumentPackageDto.InstrumentName, instrumentPackage);

            return await UploadInstrumentAsync(instrumentPackageDto.BucketPath, instrumentPackageDto.InstrumentName, instrumentPackage);
        }

        private async Task<string> DownloadInstrumentAsync(InstrumentPackageDto instrumentPackageDto)
        {
            var instrumentPackage = _fileService.GenerateUniqueInstrumentFile(instrumentPackageDto.InstrumentFile,  
                instrumentPackageDto.InstrumentName);

            return await _storageService.DownloadFromBucketAsync(instrumentPackageDto.BucketPath, 
                instrumentPackageDto.InstrumentFile,instrumentPackage);
        }

        private async Task<string> UploadInstrumentAsync(string bucketPath, string instrumentName,string instrumentFile)
        {
            var dataBucketPath = $"{bucketPath}/data/{instrumentName}";
            await _storageService.UploadToBucketAsync(dataBucketPath, instrumentFile);

            _fileService.DeleteFile(instrumentFile);

            return dataBucketPath;
        }
    }
}
