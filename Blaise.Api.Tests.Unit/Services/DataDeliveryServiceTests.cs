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
    public class DataDeliveryServiceTests
    {
        private DataDeliveryService _sut;

        private Mock<IFileService> _fileServiceMock;
        private Mock<IStorageService> _storageServiceMock;
        private MockSequence _mockSequence;

        private string _serverParkName;
        private string _instrumentName;
        private string _bucketPath;
        private string _instrumentFile;

        private InstrumentPackageDto _instrumentPackageDto;

        [SetUp]
        public void SetUpTests()
        {
            _fileServiceMock = new Mock<IFileService>(MockBehavior.Strict);
            _storageServiceMock = new Mock<IStorageService>(MockBehavior.Strict);
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

            _sut = new DataDeliveryService(
                _fileServiceMock.Object,
                _storageServiceMock.Object);
        }

        [Test]
        public async Task Given_I_Call_DeliverInstrument_Then_The_Correct_Services_Are_Called_In_The_Correct_Order()
        {
            //arrange
            const string instrumentFilePath = "d:\\temp\\OPN1234.zip";

            _storageServiceMock.InSequence(_mockSequence).Setup(s => s.DownloadFromBucketAsync(_bucketPath,
                    _instrumentFile))
                .ReturnsAsync(instrumentFilePath);

            _fileServiceMock.InSequence(_mockSequence).Setup(b => b
                .UpdateInstrumentFileWithData(_serverParkName, _instrumentName, instrumentFilePath));

            _storageServiceMock.InSequence(_mockSequence).Setup(s => s.DeleteFile(instrumentFilePath));

            //act
            await _sut.DeliverInstrumentAsync(_serverParkName, _instrumentPackageDto);

            //assert
            _storageServiceMock.Verify(v => v.DownloadFromBucketAsync(_bucketPath, _instrumentFile), Times.Once);
            _fileServiceMock.Verify(v => v.UpdateInstrumentFileWithData(_serverParkName,
                _instrumentName, instrumentFilePath), Times.Once);

            _storageServiceMock.Verify(v => v.DeleteFile(instrumentFilePath), Times.Once);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_DeliverInstrument_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            _instrumentPackageDto.InstrumentName = string.Empty;

            //act && assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await _sut.DeliverInstrumentAsync(_serverParkName,
                _instrumentPackageDto));
            Assert.AreEqual("A value for the argument 'instrumentPackageDto.InstrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_DeliverInstrument_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            _instrumentPackageDto.InstrumentName = null;

            //act && assert
            var exception = Assert.ThrowsAsync<ArgumentNullException>(async () => await _sut.DeliverInstrumentAsync(_serverParkName,
                _instrumentPackageDto));
            Assert.AreEqual("instrumentPackageDto.InstrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_DeliverInstrument_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await _sut.DeliverInstrumentAsync(string.Empty, 
                _instrumentPackageDto));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_DeliverInstrument_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.ThrowsAsync<ArgumentNullException>(async () => await _sut.DeliverInstrumentAsync(null,
                _instrumentPackageDto));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_BucketPath_When_I_Call_DeliverInstrument_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            _instrumentPackageDto.BucketPath = string.Empty;

            //act && assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await _sut.DeliverInstrumentAsync(_serverParkName, _instrumentPackageDto));
            Assert.AreEqual("A value for the argument 'instrumentPackageDto.BucketPath' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_BucketPath_When_I_Call_DeliverInstrument_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            _instrumentPackageDto.BucketPath = null;

            //act && assert
            var exception = Assert.ThrowsAsync<ArgumentNullException>(async () => await _sut.DeliverInstrumentAsync(_serverParkName,
                _instrumentPackageDto));
            Assert.AreEqual("instrumentPackageDto.BucketPath", exception.ParamName);
        }
    }
}
