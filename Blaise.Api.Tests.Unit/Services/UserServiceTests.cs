using System;
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

        private UserDto _userDto;
        private AddUserDto _addUserDto;
        private UpdateUserDto _updateUserDto;
        private UpdatePasswordDto _passwordDto;

        [SetUp]
        public void SetUpTests()
        {
            _name = "Admin";
            _password = "Test";
            _role = "Role1";
            _serverParks = new List<string> { "ServerPark1", "ServerPark2" };
            _defaultServerPark = _serverParks.First();

            _userDto = new UserDto();

            _addUserDto = new AddUserDto
            {
                Name = _name,
                Password = _password,
                Role = (_role),
                ServerParks = _serverParks,
            };

            _updateUserDto = new UpdateUserDto
            {
                Role = (_role),
                ServerParks = _serverParks,
            };

            _passwordDto = new UpdatePasswordDto
            {
                Password = _password
            };
           
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

            var userDtos = new List<UserDto> { _userDto };

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
            var userMock = new Mock<IUser>();

            _blaiseApiMock.Setup(b => b.GetUser(_name))
                .Returns(userMock.Object);

            _mapperMock.Setup(m => m.MapToUserDto(userMock.Object))
                .Returns(_userDto);
            //act
            var result = _sut.GetUser(_name);

            //assert
            Assert.IsNotNull(result);
            Assert.AreSame(_userDto, result);
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
            //act
            _sut.AddUser(_addUserDto);

            //assert
            _blaiseApiMock.Verify(v => v.AddUser(_addUserDto.Name, _addUserDto.Password, _addUserDto.Role, _addUserDto.ServerParks, _defaultServerPark), Times.Once);
        }

        [Test]
        public void Given_An_Empty_Name_When_I_Call_AddUser_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            _addUserDto.Name = string.Empty;

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.AddUser(_addUserDto));
            Assert.AreEqual("A value for the argument 'userDto.Name' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_Name_When_I_Call_AddUser_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            _addUserDto.Name = null;

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.AddUser(_addUserDto));
            Assert.AreEqual("userDto.Name", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_Password_When_I_Call_AddUser_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            _addUserDto.Password = string.Empty;

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.AddUser(_addUserDto));
            Assert.AreEqual("A value for the argument 'userDto.Password' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_Password_When_I_Call_AddUser_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            _addUserDto.Password = null;

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.AddUser(_addUserDto));
            Assert.AreEqual("userDto.Password", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_Role_When_I_Call_AddUser_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            _addUserDto.Role = string.Empty;

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.AddUser(_addUserDto));
            Assert.AreEqual("A value for the argument 'userDto.Role' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_Role_When_I_Call_AddUser_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            _addUserDto.Role = null;

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.AddUser(_addUserDto));
            Assert.AreEqual("userDto.Role", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerPark_List_When_I_Call_AddUser_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            _addUserDto.ServerParks = new List<string>();

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.AddUser(_addUserDto));
            Assert.AreEqual("A value for the argument 'userDto.ServerParks' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerPark_List_When_I_Call_AddUser_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            _addUserDto.ServerParks = null;

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.AddUser(_addUserDto));
            Assert.AreEqual("userDto.ServerParks", exception.ParamName);
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

        [Test]
        public void Given_Valid_Arguments_When_I_Call_UpdatePassword_Then_The_Correct_Service_Method_Is_Called()
        {
            //act
            _sut.UpdatePassword(_name, _passwordDto);

            //assert
            _blaiseApiMock.Verify(v => v.ChangePassword(_name, _password), Times.Once);
        }

        [Test]
        public void Given_An_Empty_Name_When_I_Call_UpdatePassword_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdatePassword(string.Empty, _passwordDto));
            Assert.AreEqual("A value for the argument 'name' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_Name_When_I_Call_UpdatePassword_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdatePassword(null, _passwordDto));
            Assert.AreEqual("name", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_Password_When_I_Call_UpdatePassword_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            _passwordDto.Password = string.Empty;

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdatePassword(_name, _passwordDto));
            Assert.AreEqual("A value for the argument 'passwordDto.Password' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_Password_When_I_Call_UpdatePassword_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            _passwordDto.Password = null;

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdatePassword(_name, _passwordDto));
            Assert.AreEqual("passwordDto.Password", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_UpdateUser_Then_The_Correct_Service_Method_Is_Called()
        {
            //act
            _sut.UpdateUser(_name, _updateUserDto);

            //assert
            _blaiseApiMock.Verify(v => v.EditUser(_name, _updateUserDto.Role, _updateUserDto.ServerParks), Times.Once);
        }

        [Test]
        public void Given_An_Empty_Name_When_I_Call_UpdateUser_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateUser(string.Empty, _updateUserDto));
            Assert.AreEqual("A value for the argument 'name' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_Name_When_I_Call_UpdateUser_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateUser(null, _updateUserDto));
            Assert.AreEqual("name", exception.ParamName);
        }

        public void Given_An_Empty_Role_When_I_Call_UpdateUser_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            _updateUserDto.Role = string.Empty;

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateUser(_name, _updateUserDto));
            Assert.AreEqual("A value for the argument 'userDto.Role' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_Role_When_I_Call_UpdateUser_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            _updateUserDto.Role = null;

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateUser(_name, _updateUserDto));
            Assert.AreEqual("userDto.Role", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerPark_List_When_I_Call_UpdateUser_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            _updateUserDto.ServerParks = new List<string>();

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateUser(_name, _updateUserDto));
            Assert.AreEqual("A value for the argument 'userDto.ServerParks' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerPark_List_When_I_Call_UpdateUser_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            _updateUserDto.ServerParks = null;

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateUser(_name, _updateUserDto));
            Assert.AreEqual("userDto.ServerParks", exception.ParamName);
        }
    }
}
