﻿using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Api.Contracts.Models.Cati;
using Blaise.Api.Contracts.Models.ServerPark;
using Blaise.Api.Core.Interfaces.Mappers;
using Blaise.Api.Core.Interfaces.Services;
using Blaise.Api.Core.Services;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Api.Tests.Unit.Services
{
    public class CatiServiceTests
    {
        private ICatiService _sut;
        private Mock<IBlaiseCatiApi> _blaiseApiMock;
        private Mock<IServerParkService> _serverParkServiceMock;
        private Mock<IInstrumentDtoMapper> _mapperMock;

        private DayBatchDto _dayBatchDto;

        [SetUp]
        public void SetUpTests()
        {
            _blaiseApiMock = new Mock<IBlaiseCatiApi>();
            _serverParkServiceMock = new Mock<IServerParkService>();
            _mapperMock = new Mock<IInstrumentDtoMapper>();

            _dayBatchDto = new DayBatchDto { DaybatchDate = DateTime.Today };

            _sut = new CatiService(
                _blaiseApiMock.Object,
                _serverParkServiceMock.Object,
                _mapperMock.Object);
        }

        [Test]
        public void Given_I_Call_GetCatiInstruments_Then_I_Get_A_List_Of_CatiInstrumentDto_Back()
        {
            //arrange
            _serverParkServiceMock.Setup(i => i.GetServerParks()).Returns(new List<ServerParkDto>());

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
            const string serverPark1 = "ServerParkA";
            const string serverPark2 = "ServerParkB";

            var serverParks = new List<ServerParkDto>
            {
                new ServerParkDto {Name = serverPark1},
                new ServerParkDto {Name = serverPark2}
            };

            _serverParkServiceMock.Setup(i => i.GetServerParks()).Returns(serverParks);

            const string instrument1 = "OPN2010A";
            var survey1Mock = new Mock<ISurvey>();
            survey1Mock.Setup(s => s.Name).Returns(instrument1);
            survey1Mock.Setup(s => s.ServerPark).Returns(serverPark1);

            const string instrument2 = "OPN2010B";
            var survey2Mock = new Mock<ISurvey>();
            survey2Mock.Setup(s => s.Name).Returns(instrument2);
            survey2Mock.Setup(s => s.ServerPark).Returns(serverPark2);

            var surveyList = new List<ISurvey>
            {
                survey1Mock.Object,
                survey2Mock.Object
            };

            _blaiseApiMock.Setup(b => b.GetInstalledSurveys(serverPark1)).Returns(surveyList);
            
            var surveyDays1 = new List<DateTime> { DateTime.Today.AddDays(-1) };
            _blaiseApiMock.Setup(b => b.GetSurveyDays(instrument1, serverPark1))
                .Returns(surveyDays1);

            var surveyDays2 = new List<DateTime> { DateTime.Today };
            _blaiseApiMock.Setup(b => b.GetSurveyDays(instrument2, serverPark2))
                .Returns(surveyDays2);

            var catiInstrument1 = new CatiInstrumentDto { Name = "OPN2010A", SurveyDays = surveyDays1 };
            _mapperMock.Setup(m => m.MapToCatiInstrumentDto(survey1Mock.Object, surveyDays1)).Returns(catiInstrument1);
            
            var catiInstrument2 = new CatiInstrumentDto { Name = "OPN2010B", SurveyDays = surveyDays2 };
            _mapperMock.Setup(m => m.MapToCatiInstrumentDto(survey2Mock.Object, surveyDays2)).Returns(catiInstrument2);

            //act
            var result = _sut.GetCatiInstruments().ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<CatiInstrumentDto>>(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(2, result.Count);
            Assert.True(result.Any(c => c.Name == instrument1 && c.SurveyDays.Any(s => s == surveyDays1.First())));
            Assert.True(result.Any(c => c.Name == instrument2 && c.SurveyDays.Any(s => s == surveyDays2.First())));
        }

        [Test]
        public void Given_A_ServerPark_When_I_Call_GetCatiInstruments_Then_I_Get_A_List_Of_CatiInstrumentDto_Back()
        {
            //arrange
            const string serverParkName = "ServerParkA";

            _blaiseApiMock.Setup(i => i.GetInstalledSurveys(serverParkName)).Returns(new List<ISurvey>());

            _blaiseApiMock.Setup(b => b.GetSurveyDays(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<DateTime>());

            _mapperMock.Setup(m => m.MapToCatiInstrumentDto(It.IsAny<ISurvey>(), It.IsAny<List<DateTime>>()))
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
            const string serverPark = "ServerParkA";

            var serverParks = new List<ServerParkDto>
            {
                new ServerParkDto {Name = serverPark},
            };

            _serverParkServiceMock.Setup(i => i.GetServerParks()).Returns(serverParks);

            const string instrument1 = "OPN2010A";
            var survey1Mock = new Mock<ISurvey>();
            survey1Mock.Setup(s => s.Name).Returns(instrument1);
            survey1Mock.Setup(s => s.ServerPark).Returns(serverPark);

            const string instrument2 = "OPN2010B";
            var survey2Mock = new Mock<ISurvey>();
            survey2Mock.Setup(s => s.Name).Returns(instrument2);
            survey2Mock.Setup(s => s.ServerPark).Returns(serverPark);

            var surveyList = new List<ISurvey>
            {
                survey1Mock.Object,
                survey2Mock.Object
            };

            _blaiseApiMock.Setup(b => b.GetInstalledSurveys(serverPark)).Returns(surveyList);


            var surveyDays1 = new List<DateTime> { DateTime.Today.AddDays(-1) };
            var surveyDays2 = new List<DateTime> { DateTime.Today };

            _blaiseApiMock.Setup(b => b.GetSurveyDays(instrument1, serverPark))
                .Returns(surveyDays1);

            _blaiseApiMock.Setup(b => b.GetSurveyDays(instrument2, serverPark))
                .Returns(surveyDays2);

            var catiInstrument1 = new CatiInstrumentDto { Name = "OPN2010A", SurveyDays = surveyDays1 };
            var catiInstrument2 = new CatiInstrumentDto { Name = "OPN2010B", SurveyDays = surveyDays2 };

            _mapperMock.Setup(m => m.MapToCatiInstrumentDto(survey1Mock.Object, surveyDays1)).Returns(catiInstrument1);
            _mapperMock.Setup(m => m.MapToCatiInstrumentDto(survey2Mock.Object, surveyDays2)).Returns(catiInstrument2);

            //act
            var result = _sut.GetCatiInstruments(serverPark).ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<CatiInstrumentDto>>(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(2, result.Count);
            Assert.True(result.Any(c => c.Name == instrument1 && c.SurveyDays.Any(s => s == surveyDays1.First())));
            Assert.True(result.Any(c => c.Name == instrument2 && c.SurveyDays.Any(s => s == surveyDays2.First())));
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
            const string instrumentName = "OPN2101A";
            const string serverParkName = "ServerParkA";

            var survey1Mock = new Mock<ISurvey>();
            survey1Mock.Setup(s => s.Name).Returns(instrumentName);
            survey1Mock.Setup(s => s.ServerPark).Returns(serverParkName);

            _blaiseApiMock.Setup(i => i.GetInstalledSurvey(instrumentName, serverParkName))
                .Returns(survey1Mock.Object);

            _blaiseApiMock.Setup(b => b.GetSurveyDays(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<DateTime>());

            _mapperMock.Setup(m => m.MapToCatiInstrumentDto(It.IsAny<ISurvey>(), It.IsAny<List<DateTime>>()))
                .Returns(new CatiInstrumentDto());

            //act
            var result = _sut.GetCatiInstrument(serverParkName, instrumentName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<CatiInstrumentDto>(result);
        }

        [Test]
        public void Given_A_ServerPark_When_I_Call_GetCatiInstrument_Then_I_Get_A_Correct_CatiInstrumentDto_Returned()
        {
            //arrange
            const string instrumentName = "OPN2010A";
            const string serverParkName = "ServerParkA";

            var serverParks = new List<ServerParkDto>
            {
                new ServerParkDto {Name = serverParkName},
            };

            var survey1Mock = new Mock<ISurvey>();
            survey1Mock.Setup(s => s.Name).Returns(instrumentName);
            survey1Mock.Setup(s => s.ServerPark).Returns(serverParkName);

            _blaiseApiMock.Setup(b => b.GetInstalledSurvey(instrumentName, serverParkName)).Returns(survey1Mock.Object);
            
            _serverParkServiceMock.Setup(i => i.GetServerParks()).Returns(serverParks);

            var surveyDays1 = new List<DateTime> { DateTime.Today.AddDays(-1) };

            _blaiseApiMock.Setup(b => b.GetSurveyDays(instrumentName, serverParkName))
                .Returns(surveyDays1);
            
            var catiInstrument1 = new CatiInstrumentDto { Name = "OPN2010A", SurveyDays = surveyDays1 };

            _mapperMock.Setup(m => m.MapToCatiInstrumentDto(survey1Mock.Object, surveyDays1)).Returns(catiInstrument1);

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
