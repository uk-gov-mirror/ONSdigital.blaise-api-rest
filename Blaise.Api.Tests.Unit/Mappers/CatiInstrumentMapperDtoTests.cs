using System;
using System.Collections.Generic;
using Blaise.Api.Contracts.Models;
using Blaise.Api.Contracts.Models.Instrument;
using Blaise.Api.Core.Mappers;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Extensions;
using NUnit.Framework;

namespace Blaise.Api.Tests.Unit.Mappers
{
    public class CatiInstrumentMapperDtoTests
    {
        private CatiInstrumentDtoMapper _sut;

        [SetUp]
        public void SetupTests()
        {
            _sut = new CatiInstrumentDtoMapper();
        }

        [Test]
        public void Given_An_Instrument_And_SurveyDays_When_I_Call_MapToCatiInstrumentDto_Then_A_CatiInstrumentDto_Is_Returned()
        {
            //arrange
            var instrumentDto = new InstrumentDto();

            //act
            var result = _sut.MapToCatiInstrumentDto(instrumentDto, new List<DateTime>());

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<CatiInstrumentDto>(result);
        }

        [Test]
        public void Given_An_Instrument_When_I_Call_MapToInstrumentDto_Then_The_Correct_InstrumentDto_Properties_Are_Mapped()
        {
            //arrange
            var instrumentDto = new InstrumentDto
            {
                Name = "OPN2021A",
                ServerParkName = "ServerParkA",
                InstallDate = DateTime.Now,
                Status = SurveyStatusType.Inactive.FullName()
            };

            //act
            var result = _sut.MapToCatiInstrumentDto(instrumentDto, new List<DateTime>());

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<CatiInstrumentDto>(result);
            Assert.AreEqual(instrumentDto.Name, result.Name);
            Assert.AreEqual(instrumentDto.ServerParkName, result.ServerParkName);
            Assert.AreEqual(instrumentDto.InstallDate, result.InstallDate);
            Assert.AreEqual(instrumentDto.Status, result.Status);
        }

        [Test]
        public void Given_SurveyDays_Have_All_Passed_When_I_Call_MapToInstrumentDto_Then_The_Instrument_Is_Marked_As_Expired()
        {
            //arrange
            var surveyDays = new List<DateTime>
            {
                DateTime.Today.AddDays(-3),
                DateTime.Today.AddDays(-2),
                DateTime.Today.AddDays(-1)
            };

            //act
            var result = _sut.MapToCatiInstrumentDto(new InstrumentDto(), surveyDays);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<CatiInstrumentDto>(result);
            Assert.IsTrue(result.Expired);
        }

        [Test]
        public void Given_There_Is_A_SurveyDay_In_The_Future_When_I_Call_MapToInstrumentDto_Then_The_Instrument_Is_Not_Marked_As_Expired()
        {
            //arrange
            var surveyDays = new List<DateTime>
            {
                DateTime.Today.AddDays(-3),
                DateTime.Today.AddDays(-2),
                DateTime.Today.AddDays(-1),
                DateTime.Today.AddDays(1)
            };

            //act
            var result = _sut.MapToCatiInstrumentDto(new InstrumentDto(), surveyDays);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<CatiInstrumentDto>(result);
            Assert.IsFalse(result.Expired);
        }

        [Test]
        public void Given_There_Is_A_SurveyDay_For_Today_When_I_Call_MapToInstrumentDto_Then_The_Instrument_Is_Not_Marked_As_Expired()
        {
            //arrange
            var surveyDays = new List<DateTime>
            {
                DateTime.Today.AddDays(-3),
                DateTime.Today.AddDays(-2),
                DateTime.Today.AddDays(-1),
                DateTime.Today
            };

            //act
            var result = _sut.MapToCatiInstrumentDto(new InstrumentDto(), surveyDays);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<CatiInstrumentDto>(result);
            Assert.IsFalse(result.Expired);
        }

        [Test]
        public void Given_There_A_SurveyDay_For_Today_At_A_Later_Time_When_I_Call_MapToInstrumentDto_Then_The_Instrument_Is_Not_Marked_As_Expired()
        {
            //arrange
            var surveyDays = new List<DateTime>
            {
                DateTime.Today.AddHours(1)
            };

            //act
            var result = _sut.MapToCatiInstrumentDto(new InstrumentDto(), surveyDays);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<CatiInstrumentDto>(result);
            Assert.IsFalse(result.Expired);
        }

        [Test]
        public void Given_No_Survey_For_Today_When_I_Call_MapToInstrumentDto_Then_The_ActiveToday_Field_Is_Marked_As_False()
        {
            //arrange
            var surveyDays = new List<DateTime>
            {
                DateTime.Today.AddDays(-3),
                DateTime.Today.AddDays(-2),
                DateTime.Today.AddDays(1)
            };

            //act
            var result = _sut.MapToCatiInstrumentDto(new InstrumentDto(), surveyDays);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<CatiInstrumentDto>(result);
            Assert.IsFalse(result.ActiveToday);
        }

        [Test]
        public void Given_There_Is_A_SurveyDay_For_Today_When_I_Call_MapToInstrumentDto_Then_The_ActiveToday_Field_Is_Marked_As_True()
        {
            //arrange
            var surveyDays = new List<DateTime>
            {
                DateTime.Today.AddDays(-3),
                DateTime.Today.AddDays(-2),
                DateTime.Today,
                DateTime.Today.AddDays(1)
            };

            //act
            var result = _sut.MapToCatiInstrumentDto(new InstrumentDto(), surveyDays);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<CatiInstrumentDto>(result);
            Assert.IsTrue(result.ActiveToday);
        }

        [Test]
        public void Given_There_A_SurveyDay_For_Today_At_A_Later_Time_When_I_Call_MapToInstrumentDto_Then_The_ActiveToday_Field_Is_Marked_As_True()
        {
            //arrange
            var surveyDays = new List<DateTime>
            {
                DateTime.Today.AddHours(1)
            };

            //act
            var result = _sut.MapToCatiInstrumentDto(new InstrumentDto(), surveyDays);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<CatiInstrumentDto>(result);
            Assert.IsTrue(result.ActiveToday);
        }
    }
}
