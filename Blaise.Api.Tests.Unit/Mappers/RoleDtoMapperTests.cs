using System.Collections.Generic;
using System.Linq;
using Blaise.Api.Contracts.Models.Role;
using Blaise.Api.Core.Mappers;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.Security;

namespace Blaise.Api.Tests.Unit.Mappers
{
    public class RoleDtoMapperTests
    {
        private RoleDtoMapper _sut;

        [SetUp]
        public void SetupTests()
        {
            _sut = new RoleDtoMapper();
        }

        [Test]
        public void Given_A_List_Of_Roles_When_I_Call_MapToRoleDtos_Then_A_Correct_List_Of_RoleDtos_Are_Returned()
        {
            //arrange
            var role1Name = "Name1";
            var role1Description = "Description1";
            var role1Mock = new Mock<IRole>();
            role1Mock.Setup(r => r.Name).Returns(role1Name);
            role1Mock.Setup(r => r.Description).Returns(role1Description);

            var permission1 = "Permission1";
            var actionPermission1Mock = new Mock<IActionPermission>();
            actionPermission1Mock.Setup(a => a.Action).Returns(permission1);
            actionPermission1Mock.Setup(a => a.Permission).Returns(PermissionStatus.Allowed);
            var actionPermissions = new List<IActionPermission> { actionPermission1Mock .Object};
            role1Mock.Setup(r => r.Permissions).Returns(actionPermissions);

            var role2Name = "Name2";
            var role2Description = "Description2";
            var role2Mock = new Mock<IRole>();
            role2Mock.Setup(r => r.Name).Returns(role2Name);
            role2Mock.Setup(r => r.Description).Returns(role2Description);

            var permission2 = "Permission2";
            var actionPermission2Mock = new Mock<IActionPermission>();
            actionPermission2Mock.Setup(a => a.Action).Returns(permission2);
            actionPermission2Mock.Setup(a => a.Permission).Returns(PermissionStatus.Allowed);
            var actionPermissions2 = new List<IActionPermission> { actionPermission2Mock.Object };
            role2Mock.Setup(r => r.Permissions).Returns(actionPermissions2);

            var roles = new List<IRole> {role1Mock.Object, role2Mock.Object};

            //act
            var result = _sut.MapToRoleDtos(roles).ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<RoleDto>>(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(2, result.Count);

            Assert.IsTrue(result.Any(r => 
                r.Name == role1Name &&
                r.Description == role1Description &&
                r.Permissions.Count() == 1 &&
                r.Permissions.Contains(permission1)));

            Assert.IsTrue(result.Any(r =>
                r.Name == role2Name &&
                r.Description == role2Description &&
                r.Permissions.Count() == 1 &&
                r.Permissions.Contains(permission2)));
        }

        [Test]
        public void Given_A_List_Of_Roles_When_I_Call_MapToRoleDtos_Then_Only_Allowed_Permissions_Are_Returned()
        {
            //arrange
            var role1Name = "Name1";
            var role1Description = "Description1";
            var role1Mock = new Mock<IRole>();
            role1Mock.Setup(r => r.Name).Returns(role1Name);
            role1Mock.Setup(r => r.Description).Returns(role1Description);

            var permission1 = "Permission1";
            var actionPermission1Mock = new Mock<IActionPermission>();
            actionPermission1Mock.Setup(a => a.Action).Returns(permission1);
            actionPermission1Mock.Setup(a => a.Permission).Returns(PermissionStatus.Disallowed);
            var actionPermissions = new List<IActionPermission> { actionPermission1Mock.Object };
            role1Mock.Setup(r => r.Permissions).Returns(actionPermissions);

            var role2Name = "Name2";
            var role2Description = "Description2";
            var role2Mock = new Mock<IRole>();
            role2Mock.Setup(r => r.Name).Returns(role2Name);
            role2Mock.Setup(r => r.Description).Returns(role2Description);

            var permission2 = "Permission2";
            var actionPermission2Mock = new Mock<IActionPermission>();
            actionPermission2Mock.Setup(a => a.Action).Returns(permission2);
            actionPermission2Mock.Setup(a => a.Permission).Returns(PermissionStatus.Allowed);
            var actionPermissions2 = new List<IActionPermission> { actionPermission2Mock.Object };
            role2Mock.Setup(r => r.Permissions).Returns(actionPermissions2);

            var roles = new List<IRole> { role1Mock.Object, role2Mock.Object };

            //act
            var result = _sut.MapToRoleDtos(roles).ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<RoleDto>>(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(2, result.Count);

            Assert.IsTrue(result.Any(r =>
                r.Name == role1Name &&
                r.Description == role1Description &&
                !r.Permissions.Any()));

            Assert.IsTrue(result.Any(r =>
                r.Name == role2Name &&
                r.Description == role2Description &&
                r.Permissions.Count() == 1 &&
                r.Permissions.Contains(permission2)));
        }

        [Test]
        public void Given_A_Role_When_I_Call_MapToRoleDto_Then_A_Correct_RoleDto_Is_Returned()
        {
            //arrange
            var role1Name = "Name1";
            var role1Description = "Description1";
            var role1Mock = new Mock<IRole>();
            role1Mock.Setup(r => r.Name).Returns(role1Name);
            role1Mock.Setup(r => r.Description).Returns(role1Description);

            var permission1 = "Permission1";
            var actionPermission1Mock = new Mock<IActionPermission>();
            actionPermission1Mock.Setup(a => a.Action).Returns(permission1);
            actionPermission1Mock.Setup(a => a.Permission).Returns(PermissionStatus.Allowed);

            var permission2 = "Permission2";
            var actionPermission2Mock = new Mock<IActionPermission>();
            actionPermission2Mock.Setup(a => a.Action).Returns(permission2);
            actionPermission2Mock.Setup(a => a.Permission).Returns(PermissionStatus.Allowed);

            var actionPermissions = new List<IActionPermission> { actionPermission1Mock.Object, actionPermission2Mock.Object };
            role1Mock.Setup(r => r.Permissions).Returns(actionPermissions);

            //act
            var result = _sut.MapToRoleDto(role1Mock.Object);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<RoleDto>(result);

            Assert.AreEqual(role1Name, result.Name);
            Assert.AreEqual(role1Description, result.Description);
            Assert.AreEqual(2, result.Permissions.Count());
            Assert.IsTrue(result.Permissions.Any(p => p == permission1));
            Assert.IsTrue(result.Permissions.Any(p => p == permission2));
        }

        [Test]
        public void Given_A_Role_When_I_Call_MapToRoleDto_Then_Only_Allowed_Permissions_Are_Returned()
        {
            //arrange
            var role1Name = "Name1";
            var role1Description = "Description1";
            var role1Mock = new Mock<IRole>();
            role1Mock.Setup(r => r.Name).Returns(role1Name);
            role1Mock.Setup(r => r.Description).Returns(role1Description);

            var permission1 = "Permission1";
            var actionPermission1Mock = new Mock<IActionPermission>();
            actionPermission1Mock.Setup(a => a.Action).Returns(permission1);
            actionPermission1Mock.Setup(a => a.Permission).Returns(PermissionStatus.Disallowed);

            var permission2 = "Permission2";
            var actionPermission2Mock = new Mock<IActionPermission>();
            actionPermission2Mock.Setup(a => a.Action).Returns(permission2);
            actionPermission2Mock.Setup(a => a.Permission).Returns(PermissionStatus.Allowed);

            var actionPermissions = new List<IActionPermission> { actionPermission1Mock.Object, actionPermission2Mock.Object };
            role1Mock.Setup(r => r.Permissions).Returns(actionPermissions);

            //act
            var result = _sut.MapToRoleDto(role1Mock.Object);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<RoleDto>(result);

            Assert.AreEqual(role1Name, result.Name);
            Assert.AreEqual(role1Description, result.Description);
            Assert.AreEqual(1, result.Permissions.Count());
            Assert.IsTrue(result.Permissions.Any(p => p == permission2));
        }
    }
}
