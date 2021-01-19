using Blaise.Api.Contracts.Interfaces;
using Blaise.Api.Providers;
using NUnit.Framework;

namespace Blaise.Api.Tests.Unit.Providers
{
    public class ConfigurationProviderTests
    {
        /// <summary>
        /// Please ensure the app.config in the test project has values that relate to the tests
        /// </summary>

        private IConfigurationProvider _sut;

        [SetUp]
        public void SetUpTests()
        {
            _sut = new ConfigurationProvider();
        }

        [Test]
        public void Given_BaseUrl_Value_Is_Set_When_I_Call_BaseUrl_I_Get_The_Correct_Value_Back()
        {
            //act
            var result = _sut.BaseUrl;

            //assert
            Assert.NotNull(result);
            Assert.AreEqual(@"http://*:90/", result);
        }

        [Test]
        public void Given_TempPath_Value_Is_Set_When_I_Call_TempPath_I_Get_The_Correct_Value_Back()
        {
            //act
            var result = _sut.TempPath;

            //assert
            Assert.NotNull(result);
            Assert.AreEqual(@"c:\Blaise\Temp", result);
        }
    }
}
