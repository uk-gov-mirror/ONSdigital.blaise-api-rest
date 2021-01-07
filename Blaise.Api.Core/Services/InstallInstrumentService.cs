using Blaise.Api.Contracts.Models.Instrument;
using Blaise.Api.Core.Extensions;
using Blaise.Api.Core.Interfaces.Services;
using Blaise.Api.Storage.Interfaces;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Interfaces;

namespace Blaise.Api.Core.Services
{
    public class InstallInstrumentService : IInstallInstrumentService
    {
        private readonly IBlaiseSurveyApi _blaiseApi;
        private readonly IStorageService _storageService;

        public InstallInstrumentService(
            IBlaiseSurveyApi blaiseApi,
            IStorageService storageService)
        {
            _blaiseApi = blaiseApi;
            _storageService = storageService;
        }

        public void InstallInstrument(string serverParkName, InstallInstrumentDto installInstrumentDto)
        {
            installInstrumentDto.InstrumentName.ThrowExceptionIfNullOrEmpty("installInstrumentDto.InstrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            installInstrumentDto.BucketPath.ThrowExceptionIfNullOrEmpty("installInstrumentDto.BucketPath");
            installInstrumentDto.InstrumentFile.ThrowExceptionIfNullOrEmpty("installInstrumentDto.InstrumentFile");

            var instrumentFile = _storageService.DownloadFromBucket(
                installInstrumentDto.BucketPath, 
                installInstrumentDto.InstrumentFile);

            _blaiseApi.InstallSurvey(
                installInstrumentDto.InstrumentName, 
                serverParkName, 
                instrumentFile, 
                SurveyInterviewType.Cati);

            _storageService.DeleteFile(instrumentFile);
        }

        public void UninstallInstrument(string instrumentName, string serverParkName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _blaiseApi.UninstallSurvey(instrumentName, serverParkName);
        }
    }
}
