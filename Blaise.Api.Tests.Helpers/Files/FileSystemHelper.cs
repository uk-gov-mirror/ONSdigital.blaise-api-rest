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
            if (!Directory.Exists(BlaiseConfigurationHelper.TempTestsPath)) return;

            foreach (var directory in Directory.GetDirectories(BlaiseConfigurationHelper.TempTestsPath))
            {
                Directory.Delete(directory, true);
            }
        }
    }
}