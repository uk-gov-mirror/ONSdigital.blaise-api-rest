using System;
using System.Threading.Tasks;
using Blaise.Api.Contracts.Models.Instrument;
using Blaise.Api.Core.Interfaces.Services;
using Blaise.Api.Core.Services;
using Blaise.Api.Storage.Interfaces;
using Moq;
using NUnit.Framework;

namespace Blaise.Api.Tests.Unit.Services
{
    public class DeliverInstrumentServiceTests
    {
        private DeliverInstrumentService _sut;

        private Mock<IBlaiseFileService> _fileServiceMock;
        private Mock<ICloudStorageService> _storageServiceMock;
        private MockSequence _mockSequence;

        private string _serverParkName;
        private string _instrumentName;
        private string _bucketPath;
        private string _instrumentFile;

        private InstrumentPackageDto _instrumentPackageDto;

        [SetUp]
        public void SetUpTests()
        {
            _fileServiceMock = new Mock<IBlaiseFileService>(MockBehavior.Strict);
            _storageServiceMock = new Mock<ICloudStorageService>(MockBehavior.Strict);
            _mockSequence = new MockSequence();

            _bucketPath = "OPN";
            _instrumentFile = "OPN1234.zip";
            _serverParkName = "ServerParkA";
            _instrumentName = "OPN2010A";

            _instrumentPackageDto = new InstrumentPackageDto
            {
                BucketPath = _bucketPath,
                InstrumentName = _instrumentName,
                InstrumentFile = _instrumentFile
            };

            _sut = new DeliverInstrumentService(
                _fileServiceMock.Object,
                _storageServiceMock.Object);
        }

        [Test]
        public async Task Given_I_Call_DeliverInstrument_Then_The_Correct_Services_Are_Called_In_The_Correct_Order()
        {
            //arrange
            const string deliveryFile = @"dd_OPN2004A_08042020_154000.zip";
            const string instrumentFilePath = @"d:\temp\dd_OPN2004A_08042020_154000.zip";

            _fileServiceMock.InSequence(_mockSequence).Setup(f => f
                    .GenerateUniqueInstrumentFile(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(deliveryFile);

            _storageServiceMock.InSequence(_mockSequence).Setup(s => s.DownloadFromBucketAsync(It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(instrumentFilePath);

            _fileServiceMock.InSequence(_mockSequence).Setup(f => f
                .UpdateInstrumentFileWithData(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            _storageServiceMock.InSequence(_mockSequence).Setup(s => s.UploadToBucketAsync(It.IsAny<string>(),
                It.IsAny<string>())).Returns(Task.FromResult(0));

            _fileServiceMock.InSequence(_mockSequence).Setup(s => s.DeleteFile(It.IsAny<string>()));

            //act
            await _sut.DeliverInstrumentWithDataAsync(_serverParkName, _instrumentPackageDto);

            //assert
            _fileServiceMock.Verify(v => v.GenerateUniqueInstrumentFile(
                _instrumentPackageDto.InstrumentFile, _instrumentPackageDto.InstrumentName), Times.Once);

            _storageServiceMock.Verify(v => v.DownloadFromBucketAsync(_bucketPath, _instrumentFile, 
                deliveryFile), Times.Once);

            _fileServiceMock.Verify(v => v.UpdateInstrumentFileWithData(_serverParkName,
                _instrumentName, instrumentFilePath), Times.Once);

            _storageServiceMock.Verify(v => v.UploadToBucketAsync(_bucketPath, instrumentFilePath), Times.Once);

            _fileServiceMock.Verify(v => v.DeleteFile(instrumentFilePath), Times.Once);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_DeliverInstrument_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            _instrumentPackageDto.InstrumentName = string.Empty;

            //act && assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await _sut.DeliverInstrumentWithDataAsync(_serverParkName,
                _instrumentPackageDto));
            Assert.AreEqual("A value for the argument 'instrumentPackageDto.InstrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_DeliverInstrument_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            _instrumentPackageDto.InstrumentName = null;

            //act && assert
            var exception = Assert.ThrowsAsync<ArgumentNullException>(async () => await _sut.DeliverInstrumentWithDataAsync(_serverParkName,
                _instrumentPackageDto));
            Assert.AreEqual("instrumentPackageDto.InstrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_DeliverInstrument_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await _sut.DeliverInstrumentWithDataAsync(string.Empty, 
                _instrumentPackageDto));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_DeliverInstrument_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.ThrowsAsync<ArgumentNullException>(async () => await _sut.DeliverInstrumentWithDataAsync(null,
                _instrumentPackageDto));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_BucketPath_When_I_Call_DeliverInstrument_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            _instrumentPackageDto.BucketPath = string.Empty;

            //act && assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await _sut.DeliverInstrumentWithDataAsync(_serverParkName, _instrumentPackageDto));
            Assert.AreEqual("A value for the argument 'instrumentPackageDto.BucketPath' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_BucketPath_When_I_Call_DeliverInstrument_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            _instrumentPackageDto.BucketPath = null;

            //act && assert
            var exception = Assert.ThrowsAsync<ArgumentNullException>(async () => await _sut.DeliverInstrumentWithDataAsync(_serverParkName,
                _instrumentPackageDto));
            Assert.AreEqual("instrumentPackageDto.BucketPath", exception.ParamName);
        }
    }
}
