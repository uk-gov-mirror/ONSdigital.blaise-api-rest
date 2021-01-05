using Blaise.Api.Core.Interfaces.Services;
using Blaise.Api.Core.Services;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Moq;
using NUnit.Framework;

namespace Blaise.Api.Tests.Unit.Services
{
    public class CaseServiceTests
    {
        private ICaseService _sut;
        private Mock<IBlaiseCaseApi> _blaiseApiMock;

        [SetUp]
        public void SetUpTests()
        {
            _blaiseApiMock = new Mock<IBlaiseCaseApi>();

            _sut = new CaseService(_blaiseApiMock.Object);
        }

        [Test]
        public void Given_I_Call_GetNumberOfCases_Then_I_Get_A_Correct_Number_Of_Cases_Returned()
        {
            //arrange
            const string instrumentName = "OPN2101A";
            const string serverParkName = "ServerParkA";
            const int numberOfCases = 10;

            _blaiseApiMock.Setup(b => b.GetNumberOfCases(instrumentName, serverParkName)).Returns(numberOfCases);

            //act
            var result = _sut.GetNumberOfCases(instrumentName, serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<int>(result);
            Assert.AreEqual(numberOfCases, result);
        }
    }
}
