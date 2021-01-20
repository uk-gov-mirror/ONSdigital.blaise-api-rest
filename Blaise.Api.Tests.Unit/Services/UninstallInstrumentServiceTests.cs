using System;
using Blaise.Api.Core.Services;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Api.Tests.Unit.Services
{
    public class UninstallInstrumentServiceTests
    {
        private UninstallInstrumentService _sut;

        private Mock<IBlaiseSurveyApi> _blaiseSurveyApiMock;
        private Mock<IBlaiseCaseApi> _blaiseCaseApiMock;

        private Mock<IDataSet> _casesMock;

        private string _serverParkName;
        private string _instrumentName;
        
        [SetUp]
        public void SetUpTests()
        {
            _blaiseSurveyApiMock = new Mock<IBlaiseSurveyApi>();
            _blaiseCaseApiMock = new Mock<IBlaiseCaseApi>();
            _casesMock = new Mock<IDataSet>();

            _serverParkName = "ServerParkA";
            _instrumentName = "OPN2010A";

            _sut = new UninstallInstrumentService(
                _blaiseSurveyApiMock.Object,
                _blaiseCaseApiMock.Object);
        }

        [Test]
        public void Given_I_Call_UninstallInstrument_Then_The_Correct_Services_Are_Called()
        {
            //arrange

            var primaryKey1 = "Key1";
            var primaryKey2 = "Key2";

            var dataRecord1Mock = new Mock<IDataRecord>(MockBehavior.Strict);
            var dataRecord2Mock = new Mock<IDataRecord>(MockBehavior.Strict);

            _blaiseCaseApiMock.Setup(b => b.GetCases(_instrumentName, _serverParkName))
                .Returns(_casesMock.Object);

            _casesMock.SetupSequence(ds => ds.EndOfSet)
                .Returns(false)
                .Returns(false)
                .Returns(true);

            _casesMock.SetupSequence(c => c.ActiveRecord)
                .Returns(dataRecord1Mock.Object)
                .Returns(dataRecord2Mock.Object);

            _blaiseCaseApiMock.Setup(b => b.GetPrimaryKeyValue(dataRecord1Mock.Object))
                .Returns(primaryKey1);
            _blaiseCaseApiMock.Setup(c => c.RemoveCase(
                primaryKey1, _instrumentName, _serverParkName));

            _blaiseCaseApiMock.Setup(b => b.GetPrimaryKeyValue(dataRecord2Mock.Object))
                .Returns(primaryKey2);
            _blaiseCaseApiMock.Setup(c => c.RemoveCase(
                primaryKey2, _instrumentName, _serverParkName));
            
            _blaiseSurveyApiMock.Setup(b => b
                .UninstallSurvey(_instrumentName, _serverParkName));

            //act
            _sut.UninstallInstrument(_instrumentName, _serverParkName);

            //assert
            _blaiseCaseApiMock.Verify(v => v.RemoveCase(primaryKey1, _instrumentName, _serverParkName));
            _blaiseCaseApiMock.Verify(v => v.RemoveCase(primaryKey2, _instrumentName, _serverParkName));
            _blaiseSurveyApiMock.Verify(v => v.UninstallSurvey(_instrumentName, _serverParkName)
                , Times.Once);
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
