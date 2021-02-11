using System;
using System.IO;
using System.Threading;
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

            try
            {
                Thread.Sleep(5000);
                DeleteDirectoryAndFilesInPath(BlaiseConfigurationHelper.TempTestsPath);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not cleanup folders, {e.Message}, {e}");
            }
        }

        public static void DeleteDirectoryAndFilesInPath(string path)
        {

            var files = Directory.GetFiles(path);
            var dirs = Directory.GetDirectories(path);

            foreach (var file in files)
            {
                Console.WriteLine($"Attempting to delete file '{file}'");
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (var dir in dirs)
            {
                Console.WriteLine($"Attempting to delete files and folders in'{dir}'");
                DeleteDirectoryAndFilesInPath(dir);
            }
        }
    }
}