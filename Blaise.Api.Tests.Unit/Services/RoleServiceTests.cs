using System;
using System.Collections.Generic;
using Blaise.Api.Contracts.Models;
using Blaise.Api.Core.Interfaces.Mappers;
using Blaise.Api.Core.Interfaces.Services;
using Blaise.Api.Core.Services;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.Security;

namespace Blaise.Api.Tests.Unit.Services
{
    public class RoleServiceTests
    {
        private IRoleService _sut;

        private Mock<IBlaiseRoleApi> _blaiseApiMock;
        private Mock<IRoleDtoMapper> _mapperMock;

        private string _name;
        private string _description;
        private List<string> _permissions;

        private RoleDto _roleDto;

        [SetUp]
        public void SetUpTests()
        {
            _name = "Admin";
            _description = "Test";
            _permissions = new List<string> { "Permission1" };

            _roleDto = new RoleDto
            {
                Name = _name,
                Description = _description,
                Permissions = _permissions
            };

            _blaiseApiMock = new Mock<IBlaiseRoleApi>();
            _mapperMock = new Mock<IRoleDtoMapper>();

            _sut = new RoleService(
                _blaiseApiMock.Object,
                _mapperMock.Object);
        }

        [Test]
        public void Given_I_Call_GetRoles_Then_I_Get_A_Correct_List_Of_RoleDtos_Back()
        {
            //arrange
            var roles = new List<IRole>();

            _blaiseApiMock.Setup(b => b.GetRoles())
                .Returns(roles);

            var roleDtos = new List<RoleDto> { _roleDto };

            _mapperMock.Setup(m => m.MapToRoleDtos(roles))
                .Returns(roleDtos);
            //act
            var result = _sut.GetRoles();

            //assert
            Assert.IsNotNull(result);
            Assert.AreSame(roleDtos, result);
        }

        [Test]
        public void Given_I_Call_GetRole_Then_I_Get_A_RoleDto_Back()
        {
            //arrange
            var roleMock = new Mock<IRole>();

            _blaiseApiMock.Setup(b => b.GetRole(_name))
                .Returns(roleMock.Object);

            _mapperMock.Setup(m => m.MapToRoleDto(roleMock.Object))
                .Returns(_roleDto);
            //act
            var result = _sut.GetRole(_name);

            //assert
            Assert.IsNotNull(result);
            Assert.AreSame(_roleDto, result);
        }

        [Test]
        public void Given_An_Empty_Name_When_I_Call_GetRole_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetRole(string.Empty));
            Assert.AreEqual("A value for the argument 'name' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_Name_When_I_Call_GetRole_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetRole(null));
            Assert.AreEqual("name", exception.ParamName);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Valid_Arguments_When_I_Call_RoleExists_Then_The_Correct_Value_Is_Returned(bool exists)
        {
            //arrange

            _blaiseApiMock.Setup(r => r.RoleExists(_name)).Returns(exists);

            //act
            var result = _sut.RoleExists(_name);

            //assert
            Assert.AreEqual(exists, result);
        }

        [Test]
        public void Given_An_Empty_Name_When_I_Call_RoleExists_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.RoleExists(string.Empty));
            Assert.AreEqual("A value for the argument 'name' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_Name_When_I_Call_RoleExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RoleExists(null));
            Assert.AreEqual("name", exception.ParamName);
        }


        [Test]
        public void Given_Valid_Arguments_When_I_Call_AddRole_Then_The_Correct_Service_Method_Is_Called()
        {
            //act
            _sut.AddRole(_roleDto);

            //assert
            _blaiseApiMock.Verify(v => v.AddRole(_name, _description, _permissions), Times.Once);
        }

        [Test]
        public void Given_An_Empty_Name_When_I_Call_AddRole_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            _roleDto.Name = string.Empty;

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.AddRole(_roleDto));
            Assert.AreEqual("A value for the argument 'RoleDto.Name' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_Name_When_I_Call_AddRole_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            _roleDto.Name = null;

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.AddRole(_roleDto));
            Assert.AreEqual("RoleDto.Name", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_RemoveRole_Then_The_Correct_Service_Method_Is_Called()
        {
            //act
            _sut.RemoveRole(_name);

            //assert
            _blaiseApiMock.Verify(v => v.RemoveRole(_name), Times.Once);
        }

        [Test]
        public void Given_An_Empty_Name_When_I_Call_RemoveRole_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.RemoveRole(string.Empty));
            Assert.AreEqual("A value for the argument 'name' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_Name_When_I_Call_RemoveRole_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RemoveRole(null));
            Assert.AreEqual("name", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_UpdateRolePermissions_Then_The_Correct_Service_Method_Is_Called()
        {
            //act
            _sut.UpdateRolePermissions(_name, _permissions);

            //assert
            _blaiseApiMock.Verify(v => v.UpdateRolePermissions(_name, _permissions), Times.Once);
        }

        [Test]
        public void Given_An_Empty_Name_When_I_Call_UpdateRolePermissions_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateRolePermissions(string.Empty, _permissions));
            Assert.AreEqual("A value for the argument 'name' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_Name_When_I_Call_UpdateRolePermissions_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateRolePermissions(null, _permissions));
            Assert.AreEqual("name", exception.ParamName);
        }
    }
}
