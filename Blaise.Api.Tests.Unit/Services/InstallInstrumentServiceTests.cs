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

        [SetUp]
        public void SetUpTests()
        {
            _blaiseApiMock = new Mock<IBlaiseSurveyApi>(MockBehavior.Strict);
            _storageServiceMock = new Mock<IStorageService>(MockBehavior.Strict);
            _mockSequence = new MockSequence();

            _sut = new InstallInstrumentService(
                _blaiseApiMock.Object,
                _storageServiceMock.Object);
        }

        [Test]
        public void Given_I_Call_InstallInstrument_Then_The_Correct_Services_Are_Called_In_The_Correct_Order()
        {
            //arrange
            var bucketPath = "OPN";
            var instrumentFileName = "OPN1234.zip";
            var instrumentFilePath = "d:\\temp\\OPN1234.zip";
            var serverParkName = "ServerParkA";

            _storageServiceMock.InSequence(_mockSequence).Setup(s => s.DownloadFromBucket(bucketPath, instrumentFileName))
                .Returns(instrumentFilePath);

            _blaiseApiMock.InSequence(_mockSequence).Setup(b => b
                .InstallSurvey(instrumentFilePath, SurveyInterviewType.Cati, serverParkName));

            _storageServiceMock.InSequence(_mockSequence).Setup(s => s.DeleteFile(instrumentFilePath));
            //act
            _sut.InstallInstrument(bucketPath, instrumentFileName, serverParkName);

            //assert
            _storageServiceMock.Verify(v => v.DownloadFromBucket(bucketPath, instrumentFileName), Times.Once);
            _blaiseApiMock.Verify(v => v.InstallSurvey(instrumentFilePath, SurveyInterviewType.Cati, serverParkName)
                , Times.Once);
            _storageServiceMock.Verify(v => v.DeleteFile(instrumentFilePath), Times.Once);

        }
    }
}
