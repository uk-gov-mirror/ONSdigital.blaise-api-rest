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
        private readonly IBlaiseFileApi _blaiseFileApi;
        private readonly IBlaiseSurveyApi _blaiseSurveyApi;
        private readonly IStorageService _storageService;

        public InstallInstrumentService(
            IBlaiseFileApi blaiseFileApi,
            IBlaiseSurveyApi blaiseApi,
            IStorageService storageService)
        {
            _blaiseFileApi = blaiseFileApi;
            _blaiseSurveyApi = blaiseApi;
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

            _blaiseFileApi.UpdateInstrumentFileWithSqlConnection(
                installInstrumentDto.InstrumentName,
                instrumentFile);

            _blaiseSurveyApi.InstallSurvey(
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

            _blaiseSurveyApi.UninstallSurvey(instrumentName, serverParkName);
        }
    }
}
