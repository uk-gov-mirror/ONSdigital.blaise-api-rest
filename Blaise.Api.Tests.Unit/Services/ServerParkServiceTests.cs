using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Api.Contracts.Models;
using Blaise.Api.Core.Interfaces;
using Blaise.Api.Core.Services;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Api.Tests.Unit.Services
{
    public class ServerParkServiceTests
    {
        private Mock<IBlaiseServerParkApi> _blaiseApiMock;
        private Mock<IServerParkDtoMapper> _mapperMock;
        private IServerParkService _sut;

        [SetUp]
        public void SetupTests()
        {
            _blaiseApiMock = new Mock<IBlaiseServerParkApi>();

            _mapperMock = new Mock<IServerParkDtoMapper>();

            _sut = new ServerParkService(_blaiseApiMock.Object, _mapperMock.Object);
        }

        [Test]
        public void Given_I_Call_GetServerParks_Then_The_Correct_Method_Is_called_On_The_Api()
        {
            //arrange
            var serverPark1Mock = new Mock<IServerPark>();
            serverPark1Mock.Setup(s => s.Name).Returns("ServerParkA");
            var serverPark2Mock = new Mock<IServerPark>();
            serverPark2Mock.Setup(s => s.Name).Returns("ServerParkB");

            var serverParkList = new List<IServerPark>
            {
                serverPark1Mock.Object,
                serverPark2Mock.Object
            };

            _blaiseApiMock.Setup(b => b.GetServerParks()).Returns(serverParkList);

            //act
            _sut.GetServerParks();

            //assert
            _blaiseApiMock.Verify(v => v.GetServerParks(), Times.Once);
            _mapperMock.Verify(v => v.MapToServerParkDtos(serverParkList));
        }

        [Test]
        public void Given_I_Call_GetServerParks_Then_The_Correct_ServerParkDto_List_Is_returned()
        {
            //arrange
            _blaiseApiMock.Setup(b => b.GetServerParks()).Returns(new List<IServerPark>());
            var serverParkDtoList = new List<ServerParkDto>
            {
                new ServerParkDto(),
                new ServerParkDto()
            };

            _mapperMock.Setup(m => m.MapToServerParkDtos(It.IsAny<List<IServerPark>>()))
                .Returns(serverParkDtoList);

            //act
            var result = _sut.GetServerParks().ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<ServerParkDto>>(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(serverParkDtoList, result);
        }

        [Test]
        public void Given_I_Call_GetServerPark_Then_The_Correct_Method_Is_called_On_The_Api()
        {
            //arrange
            var serverParkName = "ServerParkA";
            var serverPark1Mock = new Mock<IServerPark>();
            serverPark1Mock.Setup(s => s.Name).Returns(serverParkName);

            _blaiseApiMock.Setup(b => b.GetServerPark(serverParkName))
                .Returns(serverPark1Mock.Object);

            //act
            _sut.GetServerPark(serverParkName);

            //assert
            _blaiseApiMock.Verify(v => v.GetServerPark(serverParkName), Times.Once);
            _mapperMock.Verify(v => v.MapToServerParkDto(serverPark1Mock.Object));
        }

        [Test]
        public void Given_I_Call_GetServerPark_Then_The_Correct_ServerParkDto_Is_returned()
        {
            //arrange
            var serverParkName = "ServerParkA";
            var serverParkDto = new ServerParkDto();

            _blaiseApiMock.Setup(b => b.GetServerPark(serverParkName))
                .Returns(It.IsAny<IServerPark>());

            _mapperMock.Setup(m => m.MapToServerParkDto(It.IsAny<IServerPark>()))
                .Returns(serverParkDto);

            //act
            var result = _sut.GetServerPark(serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ServerParkDto>(result);
            Assert.AreEqual(serverParkDto, result);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetServerPark_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetServerPark(string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetServerPark_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetServerPark(null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_I_Call_ServerParkExists_Then_The_Correct_Method_Is_called_On_The_Api()
        {
            //arrange
            var serverParkName = "ServerParkA";

            _blaiseApiMock.Setup(b => b.ServerParkExists(serverParkName))
                .Returns(It.IsAny<bool>());

            //act
            _sut.ServerParkExists(serverParkName);

            //assert
            _blaiseApiMock.Verify(v => v.ServerParkExists(serverParkName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_I_Call_ServerParkExists_Then_The_Correct_Response_Is_returned(bool exists)
        {
            //arrange
            var serverParkName = "ServerParkA";

            _blaiseApiMock.Setup(b => b.ServerParkExists(serverParkName))
                .Returns(exists);

            //act
            var result = _sut.ServerParkExists(serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<bool>(result);
            Assert.AreEqual(exists, result);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_ServerParkExists_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.ServerParkExists(string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_ServerParkExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.ServerParkExists(null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_RegisterMachineOnServerPark_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var serverParkName = "Park1";
            var registerMachineDto = new RegisterMachineDto
            {
                MachineName = "Gusty01",
                LogicalRootName = "Default",
                Roles = new List<string> { "Web", "Cati" },
            };

            _blaiseApiMock.Setup(p => p.RegisterMachineOnServerPark(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>()));

            //act
            _sut.RegisterMachineOnServerPark(serverParkName, registerMachineDto);

            //assert
            _blaiseApiMock.Verify(v => v.RegisterMachineOnServerPark(serverParkName,
                registerMachineDto.MachineName, registerMachineDto.LogicalRootName, registerMachineDto.Roles), Times.Once);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_RegisterMachineOnServerPark_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var registerMachineDto = new RegisterMachineDto
            {
                MachineName = "Gusty01",
                LogicalRootName = "Default",
                Roles = new List<string> { "Web", "Cati" },
            };

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.RegisterMachineOnServerPark(string.Empty,
               registerMachineDto));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_RegisterMachineOnServerPark_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var registerMachineDto = new RegisterMachineDto
            {
                MachineName = "Gusty01",
                LogicalRootName = "Default",
                Roles = new List<string> { "Web", "Cati" },
            };

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RegisterMachineOnServerPark(null,
                registerMachineDto));
        }

        [Test]
        public void Given_An_Empty_MachineName_When_I_Call_RegisterMachineOnServerPark_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var serverParkName = "Park1";
            var registerMachineDto = new RegisterMachineDto
            {
                MachineName = string.Empty,
                LogicalRootName = "Default",
                Roles = new List<string> { "Web", "Cati" },
            };

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.RegisterMachineOnServerPark(serverParkName,
                registerMachineDto));
            Assert.AreEqual("A value for the argument 'registerMachineDto.MachineName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_MachineName_When_I_Call_RegisterMachineOnServerPark_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var serverParkName = "Park1";
            var registerMachineDto = new RegisterMachineDto
            {
                MachineName = null,
                LogicalRootName = "Default",
                Roles = new List<string> { "Web", "Cati" },
            };

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RegisterMachineOnServerPark(serverParkName,
                registerMachineDto));
            Assert.AreEqual("registerMachineDto.MachineName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_LogicalRootName_When_I_Call_RegisterMachineOnServerPark_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var serverParkName = "Park1";
            var registerMachineDto = new RegisterMachineDto
            {
                MachineName = "Gusty01",
                LogicalRootName = string.Empty,
                Roles = new List<string> { "Web", "Cati" },
            };

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.RegisterMachineOnServerPark(serverParkName,
                registerMachineDto));
            Assert.AreEqual("A value for the argument 'registerMachineDto.LogicalRootName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_LogicalRootName_When_I_Call_RegisterMachineOnServerPark_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var serverParkName = "Park1";
            var registerMachineDto = new RegisterMachineDto
            {
                MachineName = "Gusty01",
                LogicalRootName = null,
                Roles = new List<string> { "Web", "Cati" },
            };

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RegisterMachineOnServerPark(serverParkName,
                registerMachineDto));
            Assert.AreEqual("registerMachineDto.LogicalRootName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_List_Of_Roles_When_I_Call_RegisterMachineOnServerPark_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var serverParkName = "Park1";
            var registerMachineDto = new RegisterMachineDto
            {
                MachineName = "Gusty01",
                LogicalRootName = "Default",
                Roles = new List<string>(),
            };

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.RegisterMachineOnServerPark(serverParkName,
                registerMachineDto));
            Assert.AreEqual("A value for the argument 'registerMachineDto.Roles' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_List_Of_Roles_When_I_Call_RegisterMachineOnServerPark_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var serverParkName = "Park1";
            var registerMachineDto = new RegisterMachineDto
            {
                MachineName = "Gusty01",
                LogicalRootName = "Default",
                Roles = null,
            };

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RegisterMachineOnServerPark(serverParkName,
                registerMachineDto));
            Assert.AreEqual("registerMachineDto.Roles", exception.ParamName);
        }
    }
}
