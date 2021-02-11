using System.IO;
using Blaise.Api.Tests.Helpers.Configuration;

namespace Blaise.Api.Tests.Helpers.Files
{
    public class FileSystemHelper
    {
        private static FileSystemHelper _currentInstance;

        public static FileSystemHelper GetInstance()
        {
            return _currentInstance ?? (_currentInstance = new FileSystemHelper());
        }

        public void CleanUpTempFiles()
        {
            //if (Directory.Exists(BlaiseConfigurationHelper.TempDownloadPath))
            //{
            //    var tempFolder = Path.Combine(BlaiseConfigurationHelper.TempDownloadPath, BlaiseConfigurationHelper.InstrumentName);
            //    DirectoryInfo directory = new DirectoryInfo(tempFolder);
            //    if (directory.Exists)
            //    {
            //        setAttributesNormal(directory);
            //        Directory.Delete(tempFolder, true);
            //    }

            //    foreach (var file in Directory.GetFiles(BlaiseConfigurationHelper.TempDownloadPath))
            //    {
            //        File.Delete(file);
            //    }
            //}
        }

        public void setAttributesNormal(DirectoryInfo directory)
        {
            foreach (var subDir in directory.GetDirectories())
                setAttributesNormal(subDir);
            foreach (var file in directory.GetFiles())
            {
                file.Attributes = FileAttributes.Normal;
            }
        }
    }
}