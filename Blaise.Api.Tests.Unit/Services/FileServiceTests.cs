using System;
using Blaise.Api.Core.Services;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Moq;
using NUnit.Framework;

namespace Blaise.Api.Tests.Unit.Services
{
    public class FileServiceTests
    {
        private FileService _sut;

        private Mock<IBlaiseFileApi> _blaiseFileApiMock;

        private string _instrumentName;
        private string _instrumentFile;

        [SetUp]
        public void SetUpTests()
        {
            _blaiseFileApiMock = new Mock<IBlaiseFileApi>();

            _instrumentFile = "OPN1234.zip";
            _instrumentName = "OPN2010A";

            _sut = new FileService(_blaiseFileApiMock.Object);
        }

        [Test]
        public void Given_I_Call_UpdateInstrumentFileWithSqlConnection_Then_The_Correct_Services_Are_Called_In_The_Correct_Order()
        {
            //arrange
            _blaiseFileApiMock.Setup(b => b.UpdateInstrumentFileWithSqlConnection(
                _instrumentName, _instrumentFile));

            //act
            _sut.UpdateInstrumentFileWithSqlConnection(_instrumentName, _instrumentFile);

            //assert
            _blaiseFileApiMock.Verify(v => v.UpdateInstrumentFileWithSqlConnection(_instrumentName,
                _instrumentFile), Times.Once);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_UpdateInstrumentFileWithSqlConnection_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateInstrumentFileWithSqlConnection(
                string.Empty, _instrumentFile));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_UpdateInstrumentFileWithSqlConnection_Then_An_ArgumentNullException_Is_Thrown()
        {

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateInstrumentFileWithSqlConnection(
                null, _instrumentFile));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentFile_When_I_Call_UpdateInstrumentFileWithSqlConnection_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateInstrumentFileWithSqlConnection(
                _instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'instrumentFile' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentFile_When_I_Call_UpdateInstrumentFileWithSqlConnection_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateInstrumentFileWithSqlConnection(
                _instrumentName, null));
            Assert.AreEqual("instrumentFile", exception.ParamName);
        }
    }
}
