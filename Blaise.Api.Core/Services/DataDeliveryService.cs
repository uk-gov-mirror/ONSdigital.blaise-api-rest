using System.Threading.Tasks;
using Blaise.Api.Contracts.Models.Instrument;
using Blaise.Api.Core.Extensions;
using Blaise.Api.Core.Interfaces.Services;
using Blaise.Api.Storage.Interfaces;

namespace Blaise.Api.Core.Services
{
    public class DataDeliveryService : IDataDeliveryService
    {
        private readonly IFileService _fileService;
        private readonly IStorageService _storageService;

        public DataDeliveryService(
            IFileService fileService,
            IStorageService storageService)
        {
            _fileService = fileService;
            _storageService = storageService;
        }

        public async Task DeliverInstrumentAsync(string serverParkName, InstrumentPackageDto instrumentPackageDto)
        {
            instrumentPackageDto.InstrumentName.ThrowExceptionIfNullOrEmpty("instrumentPackageDto.InstrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            instrumentPackageDto.BucketPath.ThrowExceptionIfNullOrEmpty("instrumentPackageDto.BucketPath");
            instrumentPackageDto.InstrumentFile.ThrowExceptionIfNullOrEmpty("instrumentPackageDto.InstrumentFile");

            var instrumentFile = await _storageService.DownloadFromBucketAsync(
                instrumentPackageDto.BucketPath, 
                instrumentPackageDto.InstrumentFile);

            _fileService.UpdateInstrumentFileWithData(serverParkName,instrumentPackageDto.InstrumentName, instrumentFile);
            
            _storageService.DeleteFile(instrumentFile);
        }
    }
}
