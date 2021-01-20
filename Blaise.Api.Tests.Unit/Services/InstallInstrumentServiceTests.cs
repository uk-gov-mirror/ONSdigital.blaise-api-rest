﻿using System;
using Blaise.Api.Contracts.Models.Instrument;
using Blaise.Api.Core.Interfaces.Services;
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

        private Mock<IBlaiseSurveyApi> _blaiseSurveyApiMock;
        private Mock<IFileService> _fileServiceMock;
        private Mock<IStorageService> _storageServiceMock;
        private MockSequence _mockSequence;

        private string _serverParkName;
        private string _instrumentName;
        private string _bucketPath;
        private string _instrumentFileName;
        private InstallInstrumentDto _installInstrumentDto;

        [SetUp]
        public void SetUpTests()
        {
            _blaiseSurveyApiMock = new Mock<IBlaiseSurveyApi>(MockBehavior.Strict);
            _fileServiceMock = new Mock<IFileService>(MockBehavior.Strict);
            _storageServiceMock = new Mock<IStorageService>(MockBehavior.Strict);
            _mockSequence = new MockSequence();

            _bucketPath = "OPN";
            _instrumentFileName = "OPN1234.zip";
            _serverParkName = "ServerParkA";
            _instrumentName = "OPN2010A";

            _installInstrumentDto = new InstallInstrumentDto
            {
                BucketPath = _bucketPath,
                InstrumentName = _instrumentName,
                InstrumentFile = _instrumentFileName
            };

            _sut = new InstallInstrumentService(
                _blaiseSurveyApiMock.Object,
                _fileServiceMock.Object,
                _storageServiceMock.Object);
        }

        [Test]
        public void Given_I_Call_InstallInstrument_Then_The_Correct_Services_Are_Called_In_The_Correct_Order()
        {
            //arrange
            const string instrumentFilePath = "d:\\temp\\OPN1234.zip";

            _storageServiceMock.InSequence(_mockSequence).Setup(s => s.DownloadFromBucket(_bucketPath,
                    _instrumentFileName))
                .Returns(instrumentFilePath);

            _fileServiceMock.InSequence(_mockSequence).Setup(b => b
                .UpdateInstrumentFileWithSqlConnection(_instrumentName, instrumentFilePath));

            _blaiseSurveyApiMock.InSequence(_mockSequence).Setup(b => b
                .InstallSurvey(_instrumentName,_serverParkName, instrumentFilePath, SurveyInterviewType.Cati));

            _storageServiceMock.InSequence(_mockSequence).Setup(s => s.DeleteFile(instrumentFilePath));

            //act
            _sut.InstallInstrument(_serverParkName, _installInstrumentDto);

            //assert
            _storageServiceMock.Verify(v => v.DownloadFromBucket(_bucketPath, _instrumentFileName), Times.Once);
            _fileServiceMock.Verify(v => v.UpdateInstrumentFileWithSqlConnection(_instrumentName, instrumentFilePath), Times.Once);
            _blaiseSurveyApiMock.Verify(v => v.InstallSurvey(_instrumentName, _serverParkName,
                instrumentFilePath, SurveyInterviewType.Cati), Times.Once);
            _storageServiceMock.Verify(v => v.DeleteFile(instrumentFilePath), Times.Once);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_InstallInstrument_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            _installInstrumentDto.InstrumentName = string.Empty;

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.InstallInstrument(_serverParkName,
                _installInstrumentDto));
            Assert.AreEqual("A value for the argument 'installInstrumentDto.InstrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_InstallInstrument_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            _installInstrumentDto.InstrumentName = null;

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.InstallInstrument(_serverParkName,
                _installInstrumentDto));
            Assert.AreEqual("installInstrumentDto.InstrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_InstallInstrument_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.InstallInstrument(string.Empty, 
                _installInstrumentDto));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_InstallInstrument_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.InstallInstrument(null,
                _installInstrumentDto));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_BucketPath_When_I_Call_UninstallInstrument_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            _installInstrumentDto.BucketPath = string.Empty;

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.InstallInstrument(_serverParkName, _installInstrumentDto));
            Assert.AreEqual("A value for the argument 'installInstrumentDto.BucketPath' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_BucketPath_When_I_Call_UninstallInstrument_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            _installInstrumentDto.BucketPath = null;

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.InstallInstrument(_serverParkName,
                _installInstrumentDto));
            Assert.AreEqual("installInstrumentDto.BucketPath", exception.ParamName);
        }
    }
}
