using System;
using Blaise.Api.Core.Services;
using Blaise.Api.Storage.Interfaces;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Moq;
using NUnit.Framework;

namespace Blaise.Api.Tests.Unit.Services
{
    public class InstallInstrumentServiceTests
    {
        private InstallInstrumentService _sut;

        private Mock<IBlaiseSurveyApi> _blaiseApiMock;
        private Mock<IStorageService> _storageServiceMock;
        private MockSequence _mockSequence;

        private string _serverParkName;
        private string _instrumentName;
        private string _bucketPath;
        private string _instrumentFileName;

        [SetUp]
        public void SetUpTests()
        {
            _blaiseApiMock = new Mock<IBlaiseSurveyApi>(MockBehavior.Strict);
            _storageServiceMock = new Mock<IStorageService>(MockBehavior.Strict);
            _mockSequence = new MockSequence();

            _bucketPath = "OPN";
            _instrumentFileName = "OPN1234.zip";
            _serverParkName = "ServerParkA";
            _instrumentName = "OPN2010A";

            _sut = new InstallInstrumentService(
                _blaiseApiMock.Object,
                _storageServiceMock.Object);
        }

        [Test]
        public void Given_I_Call_InstallInstrument_Then_The_Correct_Services_Are_Called_In_The_Correct_Order()
        {
            //arrange
            var instrumentFilePath = "d:\\temp\\OPN1234.zip";

            _storageServiceMock.InSequence(_mockSequence).Setup(s => s.DownloadFromBucket(_bucketPath, 
                    _instrumentFileName))
                .Returns(instrumentFilePath);

            _blaiseApiMock.InSequence(_mockSequence).Setup(b => b
                .InstallSurvey(instrumentFilePath, SurveyInterviewType.Cati, _serverParkName));

            _storageServiceMock.InSequence(_mockSequence).Setup(s => s.DeleteFile(instrumentFilePath));
            //act
            _sut.InstallInstrument(_bucketPath, _instrumentFileName, _serverParkName);

            //assert
            _storageServiceMock.Verify(v => v.DownloadFromBucket(_bucketPath, _instrumentFileName), Times.Once);
            _blaiseApiMock.Verify(v => v.InstallSurvey(instrumentFilePath, SurveyInterviewType.Cati, _serverParkName)
                , Times.Once);
            _storageServiceMock.Verify(v => v.DeleteFile(instrumentFilePath), Times.Once);
        }

        [Test]
        public void Given_An_Empty_BucketPath_When_I_Call_UninstallInstrument_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.InstallInstrument(string.Empty,
                _instrumentFileName,_serverParkName));
            Assert.AreEqual("A value for the argument 'bucketPath' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_BucketPath_When_I_Call_UninstallInstrument_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.InstallInstrument(null,
                _instrumentFileName, _serverParkName));
            Assert.AreEqual("bucketPath", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_InstallInstrument_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.InstallInstrument(_bucketPath,
                string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentFileName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_InstallInstrument_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.InstallInstrument(_bucketPath,
                null, _serverParkName));
            Assert.AreEqual("instrumentFileName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_InstallInstrument_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.InstallInstrument(_bucketPath, 
                _instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_InstallInstrument_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.InstallInstrument(_bucketPath,
                _instrumentName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_I_Call_UninstallInstrument_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            _blaiseApiMock.Setup(b => b
                .UninstallSurvey(_instrumentName, _serverParkName));
            //act
            _sut.UninstallInstrument(_instrumentName, _serverParkName);

            //assert
            _blaiseApiMock.Verify(v => v.UninstallSurvey(_instrumentName, _serverParkName)
                ,Times.Once);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_UninstallInstrument_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UninstallInstrument(string.Empty,
                _serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_UninstallInstrument_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UninstallInstrument(null,
                _serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_UninstallInstrument_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UninstallInstrument(_instrumentName, 
                string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_UninstallInstrument_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UninstallInstrument(_instrumentName, 
                null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }
    }
}
