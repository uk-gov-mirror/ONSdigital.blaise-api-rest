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

            CaseHelper.GetInstance().CreateCasesInFile(extractedFilePath, numberOfCases);

            await UploadFilesToBucket(extractedFilePath);
        }

        public async Task<string> CreateCaseInOnlineFileAsync(int outcomeCode)
        {
            var instrumentPackage = await DownloadPackageFromBucket();
            var extractedFilePath = ExtractPackageFiles(instrumentPackage);

            var primaryKey = CaseHelper.GetInstance().CreateCaseInFile(extractedFilePath, 
                outcomeCode, ModeType.Web);

            await UploadFilesToBucket(extractedFilePath);
            
            return primaryKey;
        }
        public async Task CleanUpOnlineFiles()
        {
            await CloudStorageHelper.GetInstance().DeleteFileInBucketAsync(BlaiseConfigurationHelper.OnlineFileBucket,
                BlaiseConfigurationHelper.InstrumentName);
        }

        private async Task<string> DownloadPackageFromBucket()
        {
            return await CloudStorageHelper.GetInstance().DownloadFromBucketAsync(
                BlaiseConfigurationHelper.InstrumentPackageBucket,
                BlaiseConfigurationHelper.InstrumentPackage, BlaiseConfigurationHelper.TempDownloadPath);
        }

        private string ExtractPackageFiles(string instrumentPackage)
        {
            var extractedFilePath = Path.Combine(BlaiseConfigurationHelper.TempDownloadPath, BlaiseConfigurationHelper.InstrumentName);
            instrumentPackage.ExtractFiles(extractedFilePath);

            return extractedFilePath;
        }

        private async Task UploadFilesToBucket(string filePath)
        {
            var uploadPath = Path.Combine(BlaiseConfigurationHelper.OnlineFileBucket,
                BlaiseConfigurationHelper.InstrumentName);

            await CloudStorageHelper.GetInstance().UploadToBucketAsync(
                uploadPath, filePath);
        }
    }
}
