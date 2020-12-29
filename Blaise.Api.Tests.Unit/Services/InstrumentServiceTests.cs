using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Api.Contracts.Models;
using Blaise.Api.Contracts.Models.Instrument;
using Blaise.Api.Core.Interfaces;
using Blaise.Api.Core.Interfaces.Mappers;
using Blaise.Api.Core.Interfaces.Services;
using Blaise.Api.Core.Services;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Api.Tests.Unit.Services
{
    public class InstrumentServiceTests
    {
        private IInstrumentService _sut;

        private Mock<IBlaiseSurveyApi> _blaiseApiMock;
        private Mock<IInstrumentDtoMapper> _mapperMock;
        private string _instrumentName;
        private string _serverParkName;

        [SetUp]
        public void SetUpTests()
        {
            _blaiseApiMock = new Mock<IBlaiseSurveyApi>();
            _mapperMock = new Mock<IInstrumentDtoMapper>();

            _instrumentName = "OPN2101A";
            _serverParkName = "ServerParkA";

            _sut = new InstrumentService(
                _blaiseApiMock.Object,
                _mapperMock.Object);
        }

        [Test]
        public void Given_I_Call_GetAllInstruments_Then_I_Get_A_List_Of_InstrumentDtos_Returned()
        {
            //arrange
            var surveys = new List<ISurvey>();
            
            _blaiseApiMock.Setup(b => b.GetSurveysAcrossServerParks())
                .Returns(surveys);

            _mapperMock.Setup(m => m.MapToInstrumentDtos(surveys))
                .Returns(new List<InstrumentDto>());
            //act
            var result = _sut.GetAllInstruments();

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<InstrumentDto>>(result);
        }
        
        [Test]
        public void Given_I_Call_GetAllInstruments_Then_I_Get_A_List_Of_All_Instruments_Across_All_ServerParks()
        {
            //arrange
            var surveys = new List<ISurvey>();

            _blaiseApiMock.Setup(b => b.GetSurveysAcrossServerParks())
                .Returns(surveys);

            //act
            _sut.GetAllInstruments();

            //assert
            _blaiseApiMock.Verify(b => b.GetSurveysAcrossServerParks());
        }

        [Test]
        public void Given_I_Call_GetAllInstruments_Then_I_Get_A_Correct_List_Of_InstrumentDtos_Returned()
        {
            //arrange
            var surveys = new List<ISurvey>();
            
            _blaiseApiMock.Setup(b => b.GetSurveysAcrossServerParks())
                .Returns(surveys);

            var instrumentDtos = new List<InstrumentDto>
            {
                new InstrumentDto {Name = "OPN2010A"},
                new InstrumentDto {Name = "OPN2010B"}
            };

            _mapperMock.Setup(m => m.MapToInstrumentDtos(surveys))
                .Returns(instrumentDtos);

            //act
            var result = _sut.GetAllInstruments().ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(instrumentDtos, result);
        }

        [Test]
        public void Given_I_Call_GetInstruments_Then_I_Get_A_List_Of_InstrumentDtos_Back()
        {
            //arrange
            var instrumentDtos = new List<InstrumentDto>();
            var surveys = new List<ISurvey>();

            _blaiseApiMock.Setup(b => b.GetSurveys(_serverParkName))
                .Returns(surveys);

            _mapperMock.Setup(m => m.MapToInstrumentDtos(surveys))
                .Returns(instrumentDtos);

            //act
            var result = _sut.GetInstruments(_serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<InstrumentDto>>(result);
        }

        [Test]
        public void Given_I_Call_GetInstrument_Then_I_Get_A_Correct_List_Of_InstrumentDtos_Back()
        {
            //arrange
            var instrumentDtos = new List<InstrumentDto>();
            var surveys = new List<ISurvey>();

            _blaiseApiMock.Setup(b => b.GetSurveys(_serverParkName))
                .Returns(surveys);

            _mapperMock.Setup(m => m.MapToInstrumentDtos(surveys))
                .Returns(instrumentDtos);

            //act
            var result = _sut.GetInstruments(_serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(instrumentDtos, result);
        }
        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetInstruments_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetInstruments(string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetInstruments_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetInstruments(null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_An_Instrument_Exists_When_I_Call_GetInstrument_Then_I_Get_An_InstrumentDto_Returned()
        {
            //arrange
            var instrumentDto = new InstrumentDto();
            var surveyMock = new Mock<ISurvey>();

            _blaiseApiMock.Setup(b => b
                    .GetSurvey(_instrumentName, _serverParkName))
                .Returns(surveyMock.Object);

            _mapperMock.Setup(m => m.MapToInstrumentDto(surveyMock.Object))
                .Returns(instrumentDto);

            //act
            var result = _sut.GetInstrument(_instrumentName, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<InstrumentDto>(result);
        }

        [Test]
        public void Given_An_Instrument_Exists_When_I_Call_GetInstrument_Then_I_Get_A_Correct_InstrumentDto_Returned()
        {
            //arrange
            var instrumentName = "OPN2101A";
            var serverParkName = "ServerParkA";
            var instrumentDto = new InstrumentDto();
            var surveyMock = new Mock<ISurvey>();

            _blaiseApiMock.Setup(b => b
                .GetSurvey(instrumentName, serverParkName))
                .Returns(surveyMock.Object);

            _mapperMock.Setup(m => m.MapToInstrumentDto(surveyMock.Object))
                .Returns(instrumentDto);

            //act
            var result = _sut.GetInstrument(instrumentName, serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(instrumentDto, result);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_GetInstrument_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetInstrument(string.Empty,
                _serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_GetInstrument_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetInstrument(null,
                _serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetInstrument_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetInstrument(_instrumentName,
                string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetInstrument_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetInstrument(_instrumentName,
                null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_An_Instrument_Exists_When_I_Call_InstrumentExists_Then_True_Is_Returned()
        {
            //arrange
            var instrumentName = "OPN2101A";
            var serverParkName = "ServerParkA";
            _blaiseApiMock.Setup(b =>
                b.SurveyExists(instrumentName, serverParkName)).Returns(true);

            //act
            var result = _sut.InstrumentExists(instrumentName, serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public void Given_An_Instrument_Does_Not_Exist_When_I_Call_InstrumentExists_Then_False_Is_Returned()
        {
            //arrange
            var instrumentName = "OPN2101A";
            var serverParkName = "ServerParkA";
            _blaiseApiMock.Setup(b =>
                b.SurveyExists(instrumentName, serverParkName)).Returns(false);

            //act
            var result = _sut.InstrumentExists(instrumentName, serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_InstrumentExists_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.InstrumentExists(string.Empty,
                _serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_InstrumentExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.InstrumentExists(null,
                _serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_InstrumentExists_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.InstrumentExists(_instrumentName,
                string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_InstrumentExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.InstrumentExists(_instrumentName,
                null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }
    }
}
