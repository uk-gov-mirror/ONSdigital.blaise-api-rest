using System;
using Blaise.Api.Core.Extensions;
using Blaise.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Contracts.Interfaces;
using System.IO;
using System.IO.Abstractions;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Blaise.Api.Tests.Unit")]
namespace Blaise.Api.Core.Services
{
    public class BlaiseFileService : IBlaiseFileService
    {
        private readonly IBlaiseFileApi _blaiseFileApi;
        private readonly IFileSystem _fileSystem;

        public BlaiseFileService(
            IBlaiseFileApi blaiseFileApi, 
            IFileSystem fileSystem)
        {
            _blaiseFileApi = blaiseFileApi;
            _fileSystem = fileSystem;
        }

        public void UpdateInstrumentFileWithSqlConnection(string instrumentName, string instrumentFile)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            instrumentFile.ThrowExceptionIfNullOrEmpty("instrumentFile");

            _blaiseFileApi.UpdateInstrumentFileWithSqlConnection(
                instrumentName,
                instrumentFile);
        }

        public void UpdateInstrumentFileWithData(string serverParkName, string instrumentName, string instrumentFile)
        {
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            instrumentFile.ThrowExceptionIfNullOrEmpty("instrumentFile");

            _blaiseFileApi.UpdateInstrumentFileWithData(serverParkName, instrumentName,
                instrumentFile);
        }
        public void DeleteFile(string instrumentFile)
        {
            _fileSystem.File.Delete(instrumentFile);
        }

        public string GenerateUniqueInstrumentFile(string instrumentFile, string instrumentName)
        {
            return GenerateUniqueInstrumentFile(instrumentFile, instrumentName, DateTime.Now);
        }

        internal string GenerateUniqueInstrumentFile(string instrumentFile, string instrumentName, DateTime dateTime)
        {
            var uniqueInstrumentName = GenerateUniqueInstrumentFileName(instrumentName, dateTime);
            var fileInfo = _fileSystem.FileInfo.FromFileName(instrumentFile);
            
            return Path.Combine(fileInfo.DirectoryName, $"{uniqueInstrumentName}{fileInfo.Extension}");
        }

        internal string GenerateUniqueInstrumentFileName(string instrumentName, DateTime dateTime)
        {
            return $"dd_{instrumentName}_{dateTime:ddMMyyyy}_{dateTime:HHmmss}";
        }
    }
}
