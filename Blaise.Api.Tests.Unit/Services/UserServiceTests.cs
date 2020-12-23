﻿using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Api.Contracts.Models.User;
using Blaise.Api.Core.Interfaces;
using Blaise.Api.Core.Services;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Api.Tests.Unit.Services
{
    public class UserServiceTests
    {
        private IUserService _sut;

        private Mock<IBlaiseUserApi> _blaiseApiMock;
        private Mock<IUserDtoMapper> _mapperMock;

        private string _name;
        private string _password;
        private string _role;
        private List<string> _serverParks;
        private string _defaultServerPark;

        [SetUp]
        public void SetUpTests()
        {
            _name = "Admin";
            _password = "Test";
            _role = "Role1";
            _serverParks = new List<string> { "ServerPark1", "ServerPark2" };
            _defaultServerPark = _serverParks.First();

            _blaiseApiMock = new Mock<IBlaiseUserApi>();
            _mapperMock = new Mock<IUserDtoMapper>();

            _sut = new UserService(
                _blaiseApiMock.Object,
                _mapperMock.Object);
        }

        [Test]
        public void Given_I_Call_GetUsers_Then_I_Get_A_Correct_List_Of_UserDtos_Back()
        {
            //arrange
            var users = new List<IUser>();

            _blaiseApiMock.Setup(b => b.GetUsers())
                .Returns(users);

            var userDtos = new List<UserDto> { new UserDto() };

            _mapperMock.Setup(m => m.MapToUserDtos(users))
                .Returns(userDtos);
            //act
            var result = _sut.GetUsers();

            //assert
            Assert.IsNotNull(result);
            Assert.AreSame(userDtos, result);
        }

        [Test]
        public void Given_I_Call_GetUser_Then_I_Get_A_RoleDto_Back()
        {
            //arrange
            var userDto = new UserDto();
            var userMock = new Mock<IUser>();

            _blaiseApiMock.Setup(b => b.GetUser(_name))
                .Returns(userMock.Object);

            _mapperMock.Setup(m => m.MapToUserDto(userMock.Object))
                .Returns(userDto);
            //act
            var result = _sut.GetUser(_name);

            //assert
            Assert.IsNotNull(result);
            Assert.AreSame(userDto, result);
        }

        [Test]
        public void Given_An_Empty_Name_When_I_Call_GetUser_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetUser(string.Empty));
            Assert.AreEqual("A value for the argument 'name' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_Name_When_I_Call_GetUser_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetUser(null));
            Assert.AreEqual("name", exception.ParamName);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Valid_Arguments_When_I_Call_UserExists_Then_The_Correct_Value_Is_Returned(bool exists)
        {
            //arrange

            _blaiseApiMock.Setup(r => r.UserExists(_name)).Returns(exists);

            //act
            var result = _sut.UserExists(_name);

            //assert
            Assert.AreEqual(exists, result);
        }

        [Test]
        public void Given_An_Empty_Name_When_I_Call_UserExists_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UserExists(string.Empty));
            Assert.AreEqual("A value for the argument 'name' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_Name_When_I_Call_UserExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UserExists(null));
            Assert.AreEqual("name", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_AddUser_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var addUserDto = new AddUserDto
            {
                Name = _name,
                Password = _password,
                Role = (_role),
                ServerParks = _serverParks,
            };

            //act
            _sut.AddUser(addUserDto);

            //assert
            _blaiseApiMock.Verify(v => v.AddUser(addUserDto.Name, addUserDto.Password, addUserDto.Role, addUserDto.ServerParks, _defaultServerPark), Times.Once);
        }

        [Test]
        public void Given_An_Empty_Name_When_I_Call_AddUser_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var addUserDto = new AddUserDto
            {
                Name = string.Empty,
                Password = _password,
                Role = _role,
                ServerParks = _serverParks,
            };

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.AddUser(addUserDto));
            Assert.AreEqual("A value for the argument 'addUserDto.Name' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_Name_When_I_Call_AddUser_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var addUserDto = new AddUserDto
            {
                Name = null,
                Password = _password,
                Role = _role,
                ServerParks = _serverParks,
            };

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.AddUser(addUserDto));
            Assert.AreEqual("addUserDto.Name", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_Password_When_I_Call_AddUser_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var addUserDto = new AddUserDto
            {
                Name = _name,
                Password = string.Empty,
                Role = _role,
                ServerParks = _serverParks,
            };

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.AddUser(addUserDto));
            Assert.AreEqual("A value for the argument 'addUserDto.Password' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_Password_When_I_Call_AddUser_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var addUserDto = new AddUserDto
            {
                Name = _name,
                Password = null,
                Role = _role,
                ServerParks = _serverParks,
            };

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.AddUser(addUserDto));
            Assert.AreEqual("addUserDto.Password", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_Role_When_I_Call_AddUser_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var addUserDto = new AddUserDto
            {
                Name = _name,
                Password = _password,
                Role = string.Empty,
                ServerParks = _serverParks,
            };

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.AddUser(addUserDto));
            Assert.AreEqual("A value for the argument 'addUserDto.Role' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_Role_When_I_Call_AddUser_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var addUserDto = new AddUserDto
            {
                Name = _name,
                Password = _password,
                Role = null,
                ServerParks = _serverParks,
            };

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.AddUser(addUserDto));
            Assert.AreEqual("addUserDto.Role", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerPark_List_When_I_Call_AddUser_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var addUserDto = new AddUserDto
            {
                Name = _name,
                Password = _password,
                Role = _role,
                ServerParks = new List<string>(),
            };

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.AddUser(addUserDto));
            Assert.AreEqual("A value for the argument 'addUserDto.ServerParks' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerPark_List_When_I_Call_AddUser_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var addUserDto = new AddUserDto
            {
                Name = _name,
                Password = _password,
                Role = _role,
                ServerParks = null,
            };

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.AddUser(addUserDto));
            Assert.AreEqual("addUserDto.ServerParks", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_UpdatePassword_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var updatePasswordDto = new UpdateUserPasswordDto {Password = _password};

            //act
            _sut.UpdatePassword(_name, updatePasswordDto);

            //assert
            _blaiseApiMock.Verify(v => v.UpdatePassword(_name, _password), Times.Once);
        }

        [Test]
        public void Given_An_Empty_Name_When_I_Call_UpdatePassword_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var updatePasswordDto = new UpdateUserPasswordDto {Password = _password};

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdatePassword(string.Empty, updatePasswordDto));
            Assert.AreEqual("A value for the argument 'name' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_Name_When_I_Call_UpdatePassword_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var updatePasswordDto = new UpdateUserPasswordDto {Password = _password};

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdatePassword(null, updatePasswordDto));
            Assert.AreEqual("name", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_Password_When_I_Call_UpdatePassword_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var updatePasswordDto = new UpdateUserPasswordDto {Password = string.Empty};

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdatePassword(_name, updatePasswordDto));
            Assert.AreEqual("A value for the argument 'updateUserPasswordDto.Password' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_Password_When_I_Call_UpdatePassword_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var updatePasswordDto = new UpdateUserPasswordDto {Password = null};

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdatePassword(_name, updatePasswordDto));
            Assert.AreEqual("updateUserPasswordDto.Password", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_UpdateUser_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var updateRoleDto = new UpdateUserRoleDto {Role = _role};

            //act
            _sut.UpdateRole(_name, updateRoleDto);

            //assert
            _blaiseApiMock.Verify(v => v.UpdateRole(_name, updateRoleDto.Role), Times.Once);
        }

        [Test]
        public void Given_An_Empty_Name_When_I_Call_UpdateRole_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var updateRoleDto = new UpdateUserRoleDto {Role = _role};

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateRole(string.Empty, updateRoleDto));
            Assert.AreEqual("A value for the argument 'name' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_Name_When_I_Call_UpdateRole_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var updateUserRoleDto = new UpdateUserRoleDto {Role = _role};

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateRole(null, updateUserRoleDto));
            Assert.AreEqual("name", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_Role_When_I_Call_UpdateRole_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var updateUserRoleDto = new UpdateUserRoleDto {Role = string.Empty};

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateRole(_name, updateUserRoleDto));
            Assert.AreEqual("A value for the argument 'updateUserRoleDto.Role' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_Role_When_I_Call_UpdateRole_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var updateUserRoleDto = new UpdateUserRoleDto {Role = null};

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateRole(_name, updateUserRoleDto));
            Assert.AreEqual("updateUserRoleDto.Role", exception.ParamName);
        }

        
        [Test]
        public void Given_Valid_Arguments_When_I_Call_UpdateServerParks_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var updateServerParksDto = new UpdateUserServerParksDto {ServerParks = _serverParks};

            //act
            _sut.UpdateServerParks(_name, updateServerParksDto);

            //assert
            _blaiseApiMock.Verify(v => v.UpdateServerParks(_name, updateServerParksDto.ServerParks, updateServerParksDto.DefaultServerPark), Times.Once);
        }

        [Test]
        public void Given_An_Empty_Name_When_I_Call_UpdateServerParks_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var updateServerParksDto = new UpdateUserServerParksDto {ServerParks = _serverParks};

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateServerParks(string.Empty, updateServerParksDto));
            Assert.AreEqual("A value for the argument 'name' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_Name_When_I_Call_UpdateServerParks_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var updateUserServerParksDto = new UpdateUserServerParksDto {ServerParks = _serverParks};

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateServerParks(null, updateUserServerParksDto));
            Assert.AreEqual("name", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerPark_List_When_I_Call_UpdateServerParks_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var updateUserServerParksDto = new UpdateUserServerParksDto {ServerParks = new List<string>()};

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateServerParks(_name, updateUserServerParksDto));
            Assert.AreEqual("A value for the argument 'updateUserServerParksDto.ServerParks' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerPark_List_When_I_Call_UpdateServerParks_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var updateUserServerParksDto = new UpdateUserServerParksDto {ServerParks = null};

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateServerParks(_name, updateUserServerParksDto));
            Assert.AreEqual("updateUserServerParksDto.ServerParks", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_RemoveUser_Then_The_Correct_Service_Method_Is_Called()
        {
            //act
            _sut.RemoveUser(_name);

            //assert
            _blaiseApiMock.Verify(v => v.RemoveUser(_name), Times.Once);
        }

        [Test]
        public void Given_An_Empty_Name_When_I_Call_RemoveUser_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.RemoveUser(string.Empty));
            Assert.AreEqual("A value for the argument 'name' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_Name_When_I_Call_RemoveUser_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RemoveUser(null));
            Assert.AreEqual("name", exception.ParamName);
        }
    }
}