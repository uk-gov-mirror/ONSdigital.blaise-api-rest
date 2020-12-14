using Blaise.Api.Configuration;
using Blaise.Api.Core.Services;
using NUnit.Framework;

namespace Blaise.Api.Tests.Unit.Configuration
{
    public class UnityTests
    {
        [Test]
        public void Given_I_Call_Resolve_For_CatiService_Then_All_Dependencies_Are_Resolved()
        {
            //arrange
            var container = UnityConfig.GetConfiguredContainer();

            //act && assert
            Assert.DoesNotThrow(() => container.Resolve(typeof(CatiService), "CatiService"));
        }

        [Test]
        public void Given_I_Call_Resolve_For_InstrumentService_Then_All_Dependencies_Are_Resolved()
        {
            //arrange
            var container = UnityConfig.GetConfiguredContainer();

            //act && assert
            Assert.DoesNotThrow(() => container.Resolve(typeof(InstrumentService), "InstrumentService"));
        }

        [Test]
        public void Given_I_Call_Resolve_For_ServerParkService_Then_All_Dependencies_Are_Resolved()
        {
            //arrange
            var container = UnityConfig.GetConfiguredContainer();

            //act && assert
            Assert.DoesNotThrow(() => container.Resolve(typeof(ServerParkService), "ServerParkService"));
        }
    }
}
