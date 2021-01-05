using Blaise.Api.Core.Extensions;
using Blaise.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Contracts.Interfaces;

namespace Blaise.Api.Core.Services
{
    public class CaseService : ICaseService
    {
        private readonly IBlaiseCaseApi _blaiseApi;

        public CaseService(IBlaiseCaseApi blaiseApi)
        {
            _blaiseApi = blaiseApi;
        }
        
        public int GetNumberOfCases(string instrumentName, string serverParkName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _blaiseApi.GetNumberOfCases(instrumentName, serverParkName);
        }
    }
}
