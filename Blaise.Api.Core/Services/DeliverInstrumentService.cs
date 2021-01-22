using System.Threading.Tasks;
using Blaise.Api.Contracts.Models.Instrument;
using Blaise.Api.Core.Extensions;
using Blaise.Api.Core.Interfaces.Services;
using Blaise.Api.Storage.Interfaces;

namespace Blaise.Api.Core.Services
{
    public class DeliverInstrumentService : IDeliverInstrumentService
    {
        private readonly IBlaiseFileService _fileService;
        private readonly ICloudStorageService _storageService;

        public DeliverInstrumentService(
            IBlaiseFileService fileService,
            ICloudStorageService storageService)
        {
            _fileService = fileService;
            _storageService = storageService;
        }

        public async Task DeliverInstrumentWithDataAsync(string serverParkName, InstrumentPackageDto instrumentPackageDto)
        {
            instrumentPackageDto.InstrumentName.ThrowExceptionIfNullOrEmpty("instrumentPackageDto.InstrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            instrumentPackageDto.BucketPath.ThrowExceptionIfNullOrEmpty("instrumentPackageDto.BucketPath");
            instrumentPackageDto.InstrumentFile.ThrowExceptionIfNullOrEmpty("instrumentPackageDto.InstrumentFile");

            var instrumentFile = await DownloadInstrumentAsync(instrumentPackageDto);
      
            _fileService.UpdateInstrumentFileWithData(serverParkName, instrumentPackageDto.InstrumentName, instrumentFile);

            await UploadInstrumentAsync(instrumentPackageDto.BucketPath, instrumentPackageDto.InstrumentName, instrumentFile);
        }

        private async Task<string> DownloadInstrumentAsync(InstrumentPackageDto instrumentPackageDto)
        {
            var dataDeliveryFile = _fileService.GenerateUniqueInstrumentFile(instrumentPackageDto.InstrumentFile,  
                instrumentPackageDto.InstrumentName);

            return await _storageService.DownloadFromBucketAsync(instrumentPackageDto.BucketPath, 
                instrumentPackageDto.InstrumentFile,dataDeliveryFile);
        }

        private async Task UploadInstrumentAsync(string bucketPath, string instrumentName,string instrumentFile)
        {
            var deliveryBucketPath = $"{bucketPath}/delivery/{instrumentName}";
            await _storageService.UploadToBucketAsync(deliveryBucketPath, instrumentFile);

            _fileService.DeleteFile(instrumentFile);
        }
    }
}
