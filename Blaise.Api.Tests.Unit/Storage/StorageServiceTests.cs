using System;
using System.IO.Abstractions;
using Blaise.Api.Contracts.Interfaces;
using Blaise.Api.Storage.Interfaces;
using Blaise.Api.Storage.Services;
using Moq;
using NUnit.Framework;

namespace Blaise.Api.Tests.Unit.Storage
{
    public class StorageServiceTests
    {
        private StorageService _sut;

        private Mock<IConfigurationProvider> _configurationProviderMock;
        private Mock<ICloudStorageClientProvider> _storageProviderMock;
        private Mock<IFileSystem> _fileSystemMock;
        
        [SetUp]
        public void SetUpTests()
        {
            _configurationProviderMock = new Mock<IConfigurationProvider>();
            _storageProviderMock = new Mock<ICloudStorageClientProvider>();
            _fileSystemMock = new Mock<IFileSystem>();
       
            _sut = new StorageService(
                _configurationProviderMock.Object,
                _storageProviderMock.Object,
                _fileSystemMock.Object);
        }

        [Test]
        public void Given_I_Call_DownloadFromBucket_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            const string bucketPath = "OPN";
            const string instrumentFileName = "OPN1234.zip";
            const string tempPath = "d:\\temp";
            var filePath = $"{tempPath}\\{Guid.NewGuid()}";
    
            _fileSystemMock.Setup(s => s.Path.Combine(tempPath, It.IsAny<string>()))
                .Returns(filePath);

            _configurationProviderMock.Setup(c => c.TempDownloadPath).Returns(tempPath);
            _configurationProviderMock.Setup(c => c.TempDownloadPath).Returns(tempPath);
            _fileSystemMock.Setup(s => s.File.Delete(It.IsAny<string>()));

            //act
            _sut.DownloadFromBucket(bucketPath, instrumentFileName);

            //arrange
            _storageProviderMock.Verify(v => v.Download(bucketPath, 
                instrumentFileName, filePath));

            _storageProviderMock.Verify(v => v.Dispose(), Times.Once);
        }

        [Test]
        public void Given_I_Call_DownloadFromBucket_Then_The_Correct_File_Is_Returned()
        {
            //arrange
            const string bucketPath = "OPN";
            const string instrumentFileName = "OPN1234.zip";
            const string tempPath = "d:\\temp";
            var filePath = $"{tempPath}\\{Guid.NewGuid()}";
            var instrumentFilePath = $"{tempPath}\\{instrumentFileName}";

            _fileSystemMock.Setup(s => s.Path.Combine(tempPath, It.IsAny<string>()))
                .Returns(filePath);
            
            _fileSystemMock.Setup(s => s.Path.Combine(filePath, instrumentFileName))
                .Returns(instrumentFilePath);

            _configurationProviderMock.Setup(c => c.TempDownloadPath).Returns(tempPath);
            _storageProviderMock.Setup(s => s.Download(bucketPath, instrumentFileName,
                It.IsAny<string>()));
            _fileSystemMock.Setup(s => s.File.Delete(It.IsAny<string>()));

            //act
            var result =_sut.DownloadFromBucket(bucketPath, instrumentFileName);

            //arrange
            Assert.AreEqual(instrumentFilePath, result);
        }

        [Test]
        public void Given_I_Call_DeleteFile_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            const string instrumentFile = @"d:\\temp\\OPN2001A.zip";

            _fileSystemMock.Setup(s => s.File.Delete(It.IsAny<string>()));

            //act
            _sut.DeleteFile(instrumentFile);

            //arrange
            _fileSystemMock.Verify(f =>f.File.Delete(instrumentFile));
        }
    }
}
