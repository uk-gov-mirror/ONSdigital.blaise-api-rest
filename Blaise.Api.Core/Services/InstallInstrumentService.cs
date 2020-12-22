using Blaise.Api.Core.Extensions;
using Blaise.Api.Core.Interfaces;
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

        public void InstallInstrument(string bucketPath, string instrumentFileName, string serverParkName)
        {
            bucketPath.ThrowExceptionIfNullOrEmpty("bucketPath");
            instrumentFileName.ThrowExceptionIfNullOrEmpty("instrumentFileName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            var instrumentFile = _storageService.DownloadFromBucket(bucketPath, instrumentFileName);

            _blaiseApi.InstallSurvey(instrumentFile, SurveyInterviewType.Cati, serverParkName);
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
