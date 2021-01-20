using Blaise.Api.Core.Extensions;
using Blaise.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Contracts.Interfaces;

namespace Blaise.Api.Core.Services
{
    public class FileService : IFileService
    {
        private readonly IBlaiseFileApi _blaiseFileApi;

        public FileService(IBlaiseFileApi blaiseFileApi)
        {
            _blaiseFileApi = blaiseFileApi;
        }

        public void UpdateInstrumentFileWithSqlConnection(string instrumentName, string instrumentFile)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            instrumentFile.ThrowExceptionIfNullOrEmpty("instrumentFile");

            _blaiseFileApi.UpdateInstrumentFileWithSqlConnection(
                instrumentName,
                instrumentFile);
        }
    }
}
