using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Api.Contracts.Models.Cati;
using Blaise.Api.Contracts.Models.Instrument;
using Blaise.Api.Core.Interfaces.Mappers;
using Blaise.Api.Core.Interfaces.Services;
using Blaise.Api.Core.Services;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Moq;
using NUnit.Framework;

namespace Blaise.Api.Tests.Unit.Services
{
    public class CatiServiceTests
    {
        private ICatiService _sut;
        private Mock<IBlaiseCatiApi> _blaiseApiMock;
        private Mock<IInstrumentService> _instrumentServiceMock;
        private Mock<ICatiInstrumentDtoMapper> _mapperMock;

        private DayBatchDto _dayBatchDto;

        [SetUp]
        public void SetUpTests()
        {
            _blaiseApiMock = new Mock<IBlaiseCatiApi>();
            _instrumentServiceMock = new Mock<IInstrumentService>();
            _mapperMock = new Mock<ICatiInstrumentDtoMapper>();

            _dayBatchDto = new DayBatchDto { DaybatchDate = DateTime.Today };

            _sut = new CatiService(
                _blaiseApiMock.Object,
                _instrumentServiceMock.Object,
                _mapperMock.Object);
        }

        [Test]
        public void Given_I_Call_GetCatiInstruments_Then_I_Get_A_List_Of_CatiInstrumentDto_Back()
        {
            //arrange
            _instrumentServiceMock.Setup(i => i.GetAllInstruments()).Returns(new List<InstrumentDto>());

            _blaiseApiMock.Setup(b => b.GetSurveyDays(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<DateTime>());

            _mapperMock.Setup(m => m.MapToCatiInstrumentDto(It.IsAny<InstrumentDto>(), It.IsAny<List<DateTime>>()))
                .Returns(new CatiInstrumentDto());

            //act
            var result = _sut.GetCatiInstruments();

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<CatiInstrumentDto>>(result);
        }

        [Test]
        public void Given_I_Call_GetCatiInstruments_Then_I_Get_A_Correct_List_Of_CatiInstrumentDto_Returned()
        {
            //arrange
            var instrument1 = new InstrumentDto { Name = "OPN2010A", ServerParkName = "ServerParkA" };
            var instrument2 = new InstrumentDto { Name = "OPN2010B", ServerParkName = "ServerParkB" };

            var instrumentDtos = new List<InstrumentDto>
            {
                instrument1,
                instrument2
            };

            _instrumentServiceMock.Setup(i => i.GetAllInstruments()).Returns(instrumentDtos);

            var surveyDays1 = new List<DateTime> { DateTime.Today.AddDays(-1) };
            var surveyDays2 = new List<DateTime> { DateTime.Today };

            _blaiseApiMock.Setup(b => b.GetSurveyDays(instrument1.Name, instrument1.ServerParkName))
                .Returns(surveyDays1);
            _blaiseApiMock.Setup(b => b.GetSurveyDays(instrument2.Name, instrument2.ServerParkName))
                .Returns(surveyDays2);

            var catiInstrument1 = new CatiInstrumentDto { Name = "OPN2010A", SurveyDays = surveyDays1 };
            var catiInstrument2 = new CatiInstrumentDto { Name = "OPN2010B", SurveyDays = surveyDays2 };

            _mapperMock.Setup(m => m.MapToCatiInstrumentDto(instrument1, surveyDays1)).Returns(catiInstrument1);
            _mapperMock.Setup(m => m.MapToCatiInstrumentDto(instrument2, surveyDays2)).Returns(catiInstrument2);

            //act
            var result = _sut.GetCatiInstruments().ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<CatiInstrumentDto>>(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(2, result.Count);
            Assert.True(result.Any(c => c.Name == instrument1.Name && c.SurveyDays.Any(s => s == surveyDays1.First())));
            Assert.True(result.Any(c => c.Name == instrument2.Name && c.SurveyDays.Any(s => s == surveyDays2.First())));
        }

        [Test]
        public void Given_A_ServerPark_When_I_Call_GetCatiInstruments_Then_I_Get_A_List_Of_CatiInstrumentDto_Back()
        {
            //arrange
            var serverParkName = "ServerParkA";

            _instrumentServiceMock.Setup(i => i.GetInstruments(serverParkName)).Returns(new List<InstrumentDto>());

            _blaiseApiMock.Setup(b => b.GetSurveyDays(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<DateTime>());

            _mapperMock.Setup(m => m.MapToCatiInstrumentDto(It.IsAny<InstrumentDto>(), It.IsAny<List<DateTime>>()))
                .Returns(new CatiInstrumentDto());

            //act
            var result = _sut.GetCatiInstruments(serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<CatiInstrumentDto>>(result);
        }

        [Test]
        public void Given_A_ServerPark_When_I_Call_GetCatiInstruments_Then_I_Get_A_Correct_List_Of_CatiInstrumentDto_Returned()
        {
            //arrange
            var serverParkName = "ServerParkA";

            var instrument1 = new InstrumentDto { Name = "OPN2010A", ServerParkName = "ServerParkA" };
            var instrument2 = new InstrumentDto { Name = "OPN2010B", ServerParkName = "ServerParkB" };

            var instrumentDtos = new List<InstrumentDto>
            {
                instrument1,
                instrument2
            };

            _instrumentServiceMock.Setup(i => i.GetInstruments(serverParkName)).Returns(instrumentDtos);

            var surveyDays1 = new List<DateTime> { DateTime.Today.AddDays(-1) };
            var surveyDays2 = new List<DateTime> { DateTime.Today };

            _blaiseApiMock.Setup(b => b.GetSurveyDays(instrument1.Name, instrument1.ServerParkName))
                .Returns(surveyDays1);
            _blaiseApiMock.Setup(b => b.GetSurveyDays(instrument2.Name, instrument2.ServerParkName))
                .Returns(surveyDays2);

            var catiInstrument1 = new CatiInstrumentDto { Name = "OPN2010A", SurveyDays = surveyDays1 };
            var catiInstrument2 = new CatiInstrumentDto { Name = "OPN2010B", SurveyDays = surveyDays2 };

            _mapperMock.Setup(m => m.MapToCatiInstrumentDto(instrument1, surveyDays1)).Returns(catiInstrument1);
            _mapperMock.Setup(m => m.MapToCatiInstrumentDto(instrument2, surveyDays2)).Returns(catiInstrument2);

            //act
            var result = _sut.GetCatiInstruments(serverParkName).ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<CatiInstrumentDto>>(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(2, result.Count);
            Assert.True(result.Any(c => c.Name == instrument1.Name && c.SurveyDays.Any(s => s == surveyDays1.First())));
            Assert.True(result.Any(c => c.Name == instrument2.Name && c.SurveyDays.Any(s => s == surveyDays2.First())));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetCatiInstruments_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetCatiInstruments(string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetCatiInstruments_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetCatiInstruments(null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

   [Test]
        public void Given_Correct_Arguments_When_I_Call_GetCatiInstrument_Then_I_Get_A_CatiInstrumentDto_Back()
        {
            //arrange
            var instrumentName = "OPN2101A";
            var serverParkName = "ServerParkA";

            _instrumentServiceMock.Setup(i => i.GetInstrument(instrumentName, serverParkName))
                .Returns(new InstrumentDto());

            _blaiseApiMock.Setup(b => b.GetSurveyDays(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<DateTime>());

            _mapperMock.Setup(m => m.MapToCatiInstrumentDto(It.IsAny<InstrumentDto>(), It.IsAny<List<DateTime>>()))
                .Returns(new CatiInstrumentDto());

            //act
            var result = _sut.GetCatiInstrument(serverParkName, instrumentName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<CatiInstrumentDto>(result);
        }

        [Test]
        public void Given_A_ServerPark_When_I_Call_GetCatiInstrument_Then_I_Get_A_Correct_List_Of_CatiInstrumentDto_Returned()
        {
            //arrange
            var instrumentName = "OPN2101A";
            var serverParkName = "ServerParkA";

            var instrument1 = new InstrumentDto { Name = instrumentName, ServerParkName = serverParkName };

            _instrumentServiceMock.Setup(i => i.GetInstrument(instrumentName, serverParkName))
                .Returns(instrument1);

            var surveyDays1 = new List<DateTime> { DateTime.Today.AddDays(-1) };

            _blaiseApiMock.Setup(b => b.GetSurveyDays(instrument1.Name, instrument1.ServerParkName))
                .Returns(surveyDays1);

            var catiInstrument1 = new CatiInstrumentDto { Name = instrumentName, SurveyDays = surveyDays1 };

            _mapperMock.Setup(m => m.MapToCatiInstrumentDto(instrument1, surveyDays1)).Returns(catiInstrument1);

            //act
            var result = _sut.GetCatiInstrument(serverParkName, instrumentName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<CatiInstrumentDto>(result);
            Assert.AreSame(catiInstrument1, result);
        }

        
        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_GetCatiInstrument_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            const string serverParkName = "ServerParkA";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetCatiInstrument(serverParkName, string.Empty));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_GetCatiInstrument_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            const string serverParkName = "ServerParkA";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetCatiInstrument(serverParkName, null));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetCatiInstrument_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var instrumentName = "OPN2101A";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetCatiInstrument(string.Empty,
                instrumentName));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetCatiInstrument_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var instrumentName = "OPN2101A";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetCatiInstrument(null, instrumentName));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_A_SurveyDay_Exists_When_I_Call_CreateDayBatch_Then_The_Correct_Service_Is_Called()
        {
            //arrange
            const string instrumentName = "OPN2101A";
            const string serverParkName = "ServerParkA";

            _blaiseApiMock.Setup(b =>
                b.CreateDayBatch(instrumentName, serverParkName, _dayBatchDto.DaybatchDate));

            //act
            _sut.CreateDayBatch(instrumentName, serverParkName, _dayBatchDto);

            //assert
            _blaiseApiMock.Verify(v => v.CreateDayBatch(instrumentName, serverParkName, _dayBatchDto.DaybatchDate), Times.Once);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_CreateDayBatch_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            const string serverParkName = "ServerParkA";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CreateDayBatch(string.Empty,
                serverParkName, _dayBatchDto));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_CreateDayBatch_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            const string serverParkName = "ServerParkA";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateDayBatch(null,
                serverParkName, _dayBatchDto));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_CreateDayBatch_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            const string instrumentName = "OPN2101A";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CreateDayBatch(instrumentName,
                string.Empty, _dayBatchDto));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_CreateDayBatch_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            const string instrumentName = "OPN2101A";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateDayBatch(instrumentName,
                null, _dayBatchDto));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_A_Null_DayBatchDto_When_I_Call_CreateDayBatch_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            const string instrumentName = "OPN2101A";
            const string serverParkName = "ServerParkA";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateDayBatch(instrumentName,
                serverParkName, null));
            Assert.AreEqual("The argument 'dayBatchDto' must be supplied", exception.ParamName);
        }
    }
}
