using Blaise.Api.Contracts.Interfaces;
using Blaise.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Contracts.Interfaces;

namespace Blaise.Api.Core.Services
{
    public class CaseService : ICaseService
    {
        private readonly IBlaiseCaseApi _blaiseApi;
        private readonly IUpdateCaseService _updateCaseService;
        private readonly ILoggingService _loggingService;

        public CaseService(
            IBlaiseCaseApi blaiseApi, 
            IUpdateCaseService updateCaseService, 
            ILoggingService loggingService)
        {
            _blaiseApi = blaiseApi;
            _updateCaseService = updateCaseService;
            _loggingService = loggingService;
        }

        public void ImportOnlineDatabaseFile(string databaseFilePath, string instrumentName, string serverParkName)
        {
            var caseRecords = _blaiseApi.GetCases(databaseFilePath);

            while (!caseRecords.EndOfSet)
            {
                var newRecord = caseRecords.ActiveRecord;
                var primaryKey = _blaiseApi.GetPrimaryKeyValue(newRecord);

                if (_blaiseApi.CaseExists(primaryKey, instrumentName, serverParkName))
                {
                    _loggingService.LogInfo($"Case with serial number '{primaryKey}' exists in Blaise");

                    var existingCase = _blaiseApi.GetCase(primaryKey, instrumentName, serverParkName);
                    _updateCaseService.UpdateExistingCaseWithOnlineData(newRecord, existingCase, instrumentName, serverParkName, primaryKey);
                }
                else
                {
                    _loggingService.LogInfo($"Create case with serial number '{primaryKey}' in Blaise");

                    _blaiseApi.CreateCase(newRecord, instrumentName, serverParkName);
                }

                caseRecords.MoveNext();
            }
        }
    }
}
