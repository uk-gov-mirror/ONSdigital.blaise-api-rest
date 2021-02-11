using System;
using System.IO;
using System.Threading.Tasks;
using Blaise.Api.Tests.Helpers.Case;
using Blaise.Api.Tests.Helpers.Cloud;
using Blaise.Api.Tests.Helpers.Configuration;
using Blaise.Api.Tests.Helpers.Enums;
using Blaise.Api.Tests.Helpers.Extensions;

namespace Blaise.Api.Tests.Helpers.Files
{
    public class OnlineFileHelper
    {
        private static OnlineFileHelper _currentInstance;

        public static OnlineFileHelper GetInstance()
        {
            return _currentInstance ?? (_currentInstance = new OnlineFileHelper());
        }

        public async Task CreateCasesInOnlineFileAsync(int numberOfCases)
        {
            var instrumentPackage = await DownloadPackageFromBucket();
            var extractedFilePath = ExtractPackageFiles(instrumentPackage);
            var instrumentDatabase = Path.Combine(extractedFilePath, BlaiseConfigurationHelper.InstrumentName + ".bdix");

            CaseHelper.GetInstance().CreateCasesInFile(instrumentDatabase, numberOfCases);

            await UploadFilesToBucket(extractedFilePath);
        }

        public async Task<string> CreateCaseInOnlineFileAsync(int outcomeCode)
        {
            var instrumentPackage = await DownloadPackageFromBucket();
            var extractedFilePath = ExtractPackageFiles(instrumentPackage);
            var instrumentDatabase = Path.Combine(extractedFilePath, BlaiseConfigurationHelper.InstrumentName + ".bdix");

            var primaryKey = CaseHelper.GetInstance().CreateCaseInFile(instrumentDatabase,
                outcomeCode, ModeType.Web);

            await UploadFilesToBucket(extractedFilePath);
            Directory.Delete(extractedFilePath, true);

            return primaryKey;
        }
        public async Task CleanUpOnlineFiles()
        {
            await CloudStorageHelper.GetInstance().DeleteFilesInBucketAsync(BlaiseConfigurationHelper.OnlineFileBucket,
                BlaiseConfigurationHelper.InstrumentName);
        }

        private async Task<string> DownloadPackageFromBucket()
        {
            var downloadPath = Path.Combine(BlaiseConfigurationHelper.TempDownloadPath, Guid.NewGuid().ToString());
            return await CloudStorageHelper.GetInstance().DownloadFromBucketAsync(
                BlaiseConfigurationHelper.InstrumentPackageBucket,
                BlaiseConfigurationHelper.InstrumentFile, downloadPath);
        }

        private string ExtractPackageFiles(string instrumentPackage)
        {
            var extractedFilePath = Path.Combine(
                BlaiseConfigurationHelper.TempDownloadPath, 
                Guid.NewGuid().ToString(),
                BlaiseConfigurationHelper.InstrumentName);
            instrumentPackage.ExtractFiles(extractedFilePath);
            File.Delete(instrumentPackage);

            return extractedFilePath;
        }

        private async Task UploadFilesToBucket(string filePath)
        {
            var uploadPath = Path.Combine(BlaiseConfigurationHelper.OnlineFileBucket);

            await CloudStorageHelper.GetInstance().UploadFolderToBucketAsync(
                uploadPath, filePath);
        }
    }
}
