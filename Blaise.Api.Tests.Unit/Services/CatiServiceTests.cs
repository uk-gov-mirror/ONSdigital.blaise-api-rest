﻿using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Api.Contracts.Models;
using Blaise.Api.Core.Interfaces;
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
        private Mock<ICatiInstrumentMapper> _mapperMock;

        [SetUp]
        public void SetUpTests()
        {
            _blaiseApiMock = new Mock<IBlaiseCatiApi>();
            _instrumentServiceMock = new Mock<IInstrumentService>();
            _mapperMock = new Mock<ICatiInstrumentMapper>();

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
            var instrument1 = new InstrumentDto { Name = "OPN2010A", ServerParkName = "ServerParkA"};
            var instrument2 = new InstrumentDto { Name = "OPN2010B", ServerParkName = "ServerParkB" };

            var instrumentDtos = new List<InstrumentDto>
            {
                instrument1,
                instrument2
            };

            _instrumentServiceMock.Setup(i => i.GetAllInstruments()).Returns(instrumentDtos);

            var surveyDays1 = new List<DateTime> {DateTime.Today.AddDays(-1)};
            var surveyDays2 = new List<DateTime> { DateTime.Today };

            _blaiseApiMock.Setup(b => b.GetSurveyDays(instrument1.Name, instrument1.ServerParkName))
                .Returns(surveyDays1);
            _blaiseApiMock.Setup(b => b.GetSurveyDays(instrument2.Name, instrument2.ServerParkName))
                .Returns(surveyDays2);

            var catiInstrument1 = new CatiInstrumentDto {Name = "OPN2010A", SurveyDays = surveyDays1};
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
    }
}