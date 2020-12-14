using Blaise.Api.Configuration;
using Blaise.Api.Controllers;
using NUnit.Framework;

namespace Blaise.Api.Tests.Unit.Configuration
{
    public class UnityTests
    {
        [Test]
        public void Given_I_Call_Resolve_For_CatiController_Then_All_Dependencies_Are_Resolved()
        {
            //arrange
            var container = UnityConfig.GetConfiguredContainer();

            //act && assert
            Assert.DoesNotThrow(() => container.Resolve(typeof(CatiController), "CatiController"));
        }

        [Test]
        public void Given_I_Call_Resolve_For_InstrumentController_Then_All_Dependencies_Are_Resolved()
        {
            //arrange
            var container = UnityConfig.GetConfiguredContainer();

            //act && assert
            Assert.DoesNotThrow(() => container.Resolve(typeof(InstrumentController), "InstrumentController"));
        }

        [Test]
        public void Given_I_Call_Resolve_For_ServerParkController_Then_All_Dependencies_Are_Resolved()
        {
            //arrange
            var container = UnityConfig.GetConfiguredContainer();

            //act && assert
            Assert.DoesNotThrow(() => container.Resolve(typeof(ServerParkController), "ServerParkController"));
        }
    }
}
