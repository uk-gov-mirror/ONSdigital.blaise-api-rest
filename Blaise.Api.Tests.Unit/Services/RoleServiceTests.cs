using System.Collections.Generic;
using Blaise.Api.Contracts.Models;
using Blaise.Api.Core.Interfaces;
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

        [SetUp]
        public void SetUpTests()
        {
            _blaiseApiMock = new Mock<IBlaiseRoleApi>();
            _mapperMock = new Mock<IRoleDtoMapper>();

            _sut = new RoleService(
                _blaiseApiMock.Object,
                _mapperMock.Object);
        }

        [Test]
        public void Given_I_Call_GetRoles_Then_I_Get_A_List_Of_RoleDtos_Back()
        {
            //arrange
            var roles = new List<IRole>();

            _blaiseApiMock.Setup(b => b.GetRoles())
                .Returns(roles);

            _mapperMock.Setup(m => m.MapToRoleDtos(roles))
                .Returns(new List<RoleDto>());

            //act
            var result = _sut.GetRoles();

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<RoleDto>>(result);
        }

        [Test]
        public void Given_I_Call_GetRoles_Then_I_Get_A_Correct_List_Of_RoleDtos_Back()
        {
            //arrange
            var roles = new List<IRole>();

            _blaiseApiMock.Setup(b => b.GetRoles())
                .Returns(roles);

            var roleDtos = new List<RoleDto> { new RoleDto() };

            _mapperMock.Setup(m => m.MapToRoleDtos(roles))
                .Returns(roleDtos);
            //act
            var result = _sut.GetRoles();

            //assert
            Assert.IsNotNull(result);
            Assert.AreSame(roleDtos, result);
        }

        [Test]
        public void Given_I_Call_AddRoles_Then_The_Correct_Method_Is_Called()
        {
            //arrange
            var role1Dto = new RoleDto
            {
                Name = "Name1",
                Description = "Description1",
                Permissions = new List<string> { "Permission1", "Permission2" }
            };

            var role2Dto = new RoleDto
            {
                Name = "Name2",
                Description = "Description2",
                Permissions = new List<string> { "Permission3", "Permission4" }
            };

            var roles = new List<RoleDto> { role1Dto, role2Dto };

            //act
            _sut.AddRoles(roles);

            //assert
            _blaiseApiMock.Verify(v => v.AddRole(role1Dto.Name, role1Dto.Description, role1Dto.Permissions), Times.Once);
            _blaiseApiMock.Verify(v => v.AddRole(role2Dto.Name, role2Dto.Description, role2Dto.Permissions), Times.Once);
            _blaiseApiMock.VerifyNoOtherCalls();
        }

    }
}
