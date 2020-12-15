using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Api.Contracts.Enums;
using Blaise.Api.Contracts.Models;
using Blaise.Api.Core.Services;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.Cati.Runtime;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Api.Tests.Unit.Services
{
    public class HealthCheckServiceTests
    {
        private HealthCheckService _sut;

        private Mock<IConfigurationProvider> _configurationProviderMock;
        private Mock<IConnectedServerFactory> _connectionFactoryMock;
        private Mock<IRemoteDataServerFactory> _remoteConnectionFactoryMock;
        private Mock<ICatiManagementServerFactory> _catiManagementFactoryMock;

        private ConnectionModel _connectionModel;
        private Mock<IConnectedServer> _connectedServerMock;
        private Mock<IRemoteDataServer> _remoteDataServerMock;
        private Mock<IRemoteCatiManagementServer> _remoteCatiManagementServerMock;

        [SetUp]
        public void SetupTests()
        {
            _connectionModel = new ConnectionModel
            {
                ServerName = "ServerA",
                UserName = "TestUser",
                Password = "TestPassword",
                Binding = "http",
                Port = 1,
                RemotePort = 2,
                ConnectionExpiresInMinutes = 90
            };

            _configurationProviderMock= new Mock<IConfigurationProvider>();
            _configurationProviderMock.Setup(c => c.GetConnectionModel()).Returns(_connectionModel);

            _connectionFactoryMock = new Mock<IConnectedServerFactory>();
            _connectedServerMock = new Mock<IConnectedServer>();
            _connectionFactoryMock.Setup(c => c.GetConnection(_connectionModel)).Returns(_connectedServerMock.Object);

            _remoteConnectionFactoryMock = new Mock<IRemoteDataServerFactory>();
            _remoteDataServerMock = new Mock<IRemoteDataServer>();
            _remoteConnectionFactoryMock.Setup(r => r.GetConnection(_connectionModel))
                .Returns(_remoteDataServerMock.Object);

            _catiManagementFactoryMock = new Mock<ICatiManagementServerFactory>();
            _remoteCatiManagementServerMock = new Mock<IRemoteCatiManagementServer>();
            _catiManagementFactoryMock.Setup(c => c.GetConnection(_connectionModel))
                .Returns(_remoteCatiManagementServerMock.Object);

            _sut = new HealthCheckService(
                _configurationProviderMock.Object,
                _connectionFactoryMock.Object,
                _remoteConnectionFactoryMock.Object,
                _catiManagementFactoryMock.Object);
        }

        [Test]
        public void Given_Blaise_Is_Up_When_I_Call_PerformCheck_Then_A_List_Of_HealthResultDtos_Are_Returned()
        {
            //act
            var result = _sut.PerformCheck().ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<HealthCheckResultDto>>(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(4, result.Count);
        }

        [Test]
        public void Given_Blaise_Is_Up_When_I_Call_PerformCheck_Then_A_Correct_List_Of_HealthResultDtos_Are_Returned()
        {
            //act
            var result = _sut.PerformCheck().ToList();

            //assert
            Assert.IsNotNull(result);

            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.ConnectionModel && r.StatusType == HealthStatusType.Ok));
            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.Connection && r.StatusType == HealthStatusType.Ok));
            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.RemoteDataServer && r.StatusType == HealthStatusType.Ok));
            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.RemoteCatiManagement && r.StatusType == HealthStatusType.Ok));
        }

        [Test]
        public void Given_An_Invalid_ServerName_In_A_ConnectionModel_When_I_Call_PerformCheck_Then_A_Correct_List_Of_HealthResultDtos_Are_Returned()
        {
            //arrange
            _connectionModel.ServerName = string.Empty;

            //act
            var result = _sut.PerformCheck().ToList();

            //assert
            Assert.IsNotNull(result);

            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.ConnectionModel && r.StatusType == HealthStatusType.NotOk));
            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.Connection && r.StatusType == HealthStatusType.Ok));
            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.RemoteDataServer && r.StatusType == HealthStatusType.Ok));
            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.RemoteCatiManagement && r.StatusType == HealthStatusType.Ok));
        }

        [Test]
        public void Given_An_Invalid_UserName_In_A_ConnectionModel_When_I_Call_PerformCheck_Then_A_Correct_List_Of_HealthResultDtos_Are_Returned()
        {
            //arrange
            _connectionModel.UserName = string.Empty;

            //act
            var result = _sut.PerformCheck().ToList();

            //assert
            Assert.IsNotNull(result);

            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.ConnectionModel && r.StatusType == HealthStatusType.NotOk));
            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.Connection && r.StatusType == HealthStatusType.Ok));
            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.RemoteDataServer && r.StatusType == HealthStatusType.Ok));
            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.RemoteCatiManagement && r.StatusType == HealthStatusType.Ok));
        }

        [Test]
        public void Given_An_Invalid_Password_In_A_ConnectionModel_When_I_Call_PerformCheck_Then_A_Correct_List_Of_HealthResultDtos_Are_Returned()
        {
            //arrange
            _connectionModel.Password = string.Empty;

            //act
            var result = _sut.PerformCheck().ToList();

            //assert
            Assert.IsNotNull(result);

            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.ConnectionModel && r.StatusType == HealthStatusType.NotOk));
            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.Connection && r.StatusType == HealthStatusType.Ok));
            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.RemoteDataServer && r.StatusType == HealthStatusType.Ok));
            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.RemoteCatiManagement && r.StatusType == HealthStatusType.Ok));
        }

        [Test]
        public void Given_An_Invalid_Binding_In_A_ConnectionModel_When_I_Call_PerformCheck_Then_A_Correct_List_Of_HealthResultDtos_Are_Returned()
        {
            //arrange
            _connectionModel.Binding = string.Empty;

            //act
            var result = _sut.PerformCheck().ToList();

            //assert
            Assert.IsNotNull(result);

            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.ConnectionModel && r.StatusType == HealthStatusType.NotOk));
            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.Connection && r.StatusType == HealthStatusType.Ok));
            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.RemoteDataServer && r.StatusType == HealthStatusType.Ok));
            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.RemoteCatiManagement && r.StatusType == HealthStatusType.Ok));
        }

        [Test]
        public void Given_An_Invalid_Port_In_A_ConnectionModel_When_I_Call_PerformCheck_Then_A_Correct_List_Of_HealthResultDtos_Are_Returned()
        {
            //arrange
            _connectionModel.Port = 0;

            //act
            var result = _sut.PerformCheck().ToList();

            //assert
            Assert.IsNotNull(result);

            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.ConnectionModel && r.StatusType == HealthStatusType.NotOk));
            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.Connection && r.StatusType == HealthStatusType.Ok));
            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.RemoteDataServer && r.StatusType == HealthStatusType.Ok));
            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.RemoteCatiManagement && r.StatusType == HealthStatusType.Ok));
        }

        [Test]
        public void Given_An_Invalid_RemotePort_In_A_ConnectionModel_When_I_Call_PerformCheck_Then_A_Correct_List_Of_HealthResultDtos_Are_Returned()
        {
            //arrange
            _connectionModel.RemotePort = 0;

            //act
            var result = _sut.PerformCheck().ToList();

            //assert
            Assert.IsNotNull(result);

            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.ConnectionModel && r.StatusType == HealthStatusType.NotOk));
            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.Connection && r.StatusType == HealthStatusType.Ok));
            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.RemoteDataServer && r.StatusType == HealthStatusType.Ok));
            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.RemoteCatiManagement && r.StatusType == HealthStatusType.Ok));
        }

        [Test]
        public void Given_Blaise_Connection_Is_Down_When_I_Call_PerformCheck_Then_A_Correct_List_Of_HealthResultDtos_Are_Returned()
        {
            //arrange
            _connectionFactoryMock.Setup(c => c.GetConnection(_connectionModel)).Throws(new Exception());

            //act
            var result = _sut.PerformCheck().ToList();

            //assert
            Assert.IsNotNull(result);

            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.ConnectionModel && r.StatusType == HealthStatusType.Ok));
            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.Connection && r.StatusType == HealthStatusType.NotOk));
            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.RemoteDataServer && r.StatusType == HealthStatusType.Ok));
            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.RemoteCatiManagement && r.StatusType == HealthStatusType.Ok));
        }

        [Test]
        public void Given_Blaise_Remote_Connection_Is_Down_When_I_Call_PerformCheck_Then_A_Correct_List_Of_HealthResultDtos_Are_Returned()
        {
            //arrange
            _remoteConnectionFactoryMock.Setup(c => c.GetConnection(_connectionModel)).Throws(new Exception());

            //act
            var result = _sut.PerformCheck().ToList();

            //assert
            Assert.IsNotNull(result);

            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.ConnectionModel && r.StatusType == HealthStatusType.Ok));
            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.Connection && r.StatusType == HealthStatusType.Ok));
            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.RemoteDataServer && r.StatusType == HealthStatusType.NotOk));
            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.RemoteCatiManagement && r.StatusType == HealthStatusType.Ok));
        }

        [Test]
        public void Given_Blaise_Remote_Cati_Connection_Is_Down_When_I_Call_PerformCheck_Then_A_Correct_List_Of_HealthResultDtos_Are_Returned()
        {
            //arrange
            _catiManagementFactoryMock.Setup(c => c.GetConnection(_connectionModel)).Throws(new Exception());

            //act
            var result = _sut.PerformCheck().ToList();

            //assert
            Assert.IsNotNull(result);

            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.ConnectionModel && r.StatusType == HealthStatusType.Ok));
            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.Connection && r.StatusType == HealthStatusType.Ok));
            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.RemoteDataServer && r.StatusType == HealthStatusType.Ok));
            Assert.IsTrue(result.Any(r => r.CheckType == HealthCheckType.RemoteCatiManagement && r.StatusType == HealthStatusType.NotOk));
        }
    }
}
