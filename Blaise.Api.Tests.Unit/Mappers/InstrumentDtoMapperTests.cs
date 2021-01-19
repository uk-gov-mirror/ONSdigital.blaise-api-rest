using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Api.Contracts.Models.Instrument;
using Blaise.Api.Core.Mappers;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Extensions;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Api.Tests.Unit.Mappers
{
    public class InstrumentDtoMapperTests
    {
        private InstrumentDtoMapper _sut;

        [SetUp]
        public void SetupTests()
        {
            _sut = new InstrumentDtoMapper();
        }

        [Test]
        public void Given_A_List_Of_Surveys_When_I_Call_MapToInstrumentDtos_Then_The_Correct_Properties_Are_Mapped()
        {
            //arrange
            const string instrument1Name = "OPN2010A";
            const string instrument2Name = "OPN2010B";

            const string serverPark1Name = "ServerParkA";
            const string serverPark2Name = "ServerParkB";

            const int numberOfRecordForInstrument1 = 20;
            const int numberOfRecordForInstrument2 = 0;

            var survey1Mock = new Mock<ISurvey>();
            survey1Mock.As<ISurvey2>();
            survey1Mock.Setup(s => s.Name).Returns(instrument1Name);
            survey1Mock.Setup(s => s.ServerPark).Returns(serverPark1Name);
            survey1Mock.Setup(s => s.Status).Returns(SurveyStatusType.Active.FullName());

            var surveyReportingInfoMock1 = new Mock<ISurveyReportingInfo>();
            surveyReportingInfoMock1.Setup(r => r.DataRecordCount).Returns(numberOfRecordForInstrument1);
            survey1Mock.As<ISurvey2>().Setup(s => s.GetReportingInfo()).Returns(surveyReportingInfoMock1.Object);

            var survey2Mock = new Mock<ISurvey>();
            survey2Mock.As<ISurvey2>();
            survey2Mock.Setup(s => s.Name).Returns(instrument2Name);
            survey2Mock.Setup(s => s.ServerPark).Returns(serverPark2Name);
            survey2Mock.Setup(s => s.Status).Returns(SurveyStatusType.Inactive.FullName());

            var surveyReportingInfoMock2 = new Mock<ISurveyReportingInfo>();
            surveyReportingInfoMock2.Setup(r => r.DataRecordCount).Returns(numberOfRecordForInstrument2);
            survey2Mock.As<ISurvey2>().Setup(s => s.GetReportingInfo()).Returns(surveyReportingInfoMock2.Object);

            var surveys = new List<ISurvey>
            {
                survey1Mock.Object,
                survey2Mock.Object
            };

            //act
            var result = _sut.MapToInstrumentDtos(surveys).ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<InstrumentDto>>(result);
            Assert.AreEqual(2, result.Count);

            Assert.True(result.Any(i => 
                i.Name == instrument1Name && 
                i.ServerParkName == serverPark1Name && 
                i.Status == SurveyStatusType.Active.FullName() &&
                i.DataRecordCount == numberOfRecordForInstrument1 &&
                i.HasData));
            
            Assert.True(result.Any(i => 
                i.Name == instrument2Name && 
                i.ServerParkName == serverPark2Name && 
                i.Status == SurveyStatusType.Inactive.FullName() &&
                i.DataRecordCount == numberOfRecordForInstrument2 &&
                i.HasData == false));
        }

        [TestCase(0, false)]
        [TestCase(1, true)]
        [TestCase(100, true)]
        public void Given_A_Survey_When_I_Call_MapToInstrumentDto_Then_The_Correct_Properties_Are_Mapped(int numberOfRecordForInstrument,
            bool hasData)
        {
            //arrange
            const string instrumentName = "OPN2010A";
            const string serverParkName = "ServerParkA";
            var installDate = DateTime.Now;

            var surveyMock = new Mock<ISurvey>();
            surveyMock.Setup(s => s.Name).Returns(instrumentName);
            surveyMock.Setup(s => s.ServerPark).Returns(serverParkName);
            surveyMock.Setup(s => s.InstallDate).Returns(installDate);


            var surveyReportingInfoMock = new Mock<ISurveyReportingInfo>();
            surveyReportingInfoMock.Setup(r => r.DataRecordCount).Returns(numberOfRecordForInstrument);
            surveyMock.As<ISurvey2>().Setup(s => s.GetReportingInfo()).Returns(surveyReportingInfoMock.Object);

            //act
            var result = _sut.MapToInstrumentDto(surveyMock.Object);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<InstrumentDto>(result);
            Assert.AreEqual(instrumentName, result.Name);
            Assert.AreEqual(serverParkName, result.ServerParkName);
            Assert.AreEqual(installDate, result.InstallDate);
            Assert.AreEqual(numberOfRecordForInstrument, result.DataRecordCount);
            Assert.AreEqual(hasData, result.HasData);
        }
    }
}
