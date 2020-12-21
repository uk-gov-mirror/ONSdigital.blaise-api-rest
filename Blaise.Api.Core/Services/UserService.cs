using System.Collections.Generic;
using System.Linq;
using Blaise.Api.Contracts.Models.User;
using Blaise.Api.Core.Extensions;
using Blaise.Api.Core.Interfaces;
using Blaise.Nuget.Api.Contracts.Interfaces;

namespace Blaise.Api.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IBlaiseUserApi _blaiseApi;
        private readonly IUserDtoMapper _mapper;

        public UserService(
            IBlaiseUserApi blaiseApi, 
            IUserDtoMapper mapper)
        {
            _blaiseApi = blaiseApi;
            _mapper = mapper;
        }

        public IEnumerable<UserDto> GetUsers()
        {
            var user = _blaiseApi.GetUsers();

            return _mapper.MapToUserDtos(user);
        }

        public UserDto GetUser(string name)
        {
            name.ThrowExceptionIfNullOrEmpty("name");
            var user = _blaiseApi.GetUser(name);

            return _mapper.MapToUserDto(user);
        }

        public bool UserExists(string name)
        {
            name.ThrowExceptionIfNullOrEmpty("name");

            return _blaiseApi.UserExists(name);
        }

        public void AddUser(AddUserDto userDto)
        {
            userDto.Name.ThrowExceptionIfNullOrEmpty("userDto.Name");
            userDto.Password.ThrowExceptionIfNullOrEmpty("userDto.Password");
            userDto.Role.ThrowExceptionIfNullOrEmpty("userDto.Role");
            userDto.ServerParks.ThrowExceptionIfNullOrEmpty("userDto.ServerParks");

            _blaiseApi.AddUser(userDto.Name, userDto.Password, userDto.Role, userDto.ServerParks, userDto.ServerParks.FirstOrDefault());
        }

        public void RemoveUser(string name)
        {
            name.ThrowExceptionIfNullOrEmpty("name");

            _blaiseApi.RemoveUser(name);
        }

        public void UpdatePassword(string name, UpdatePasswordDto passwordDto)
        {
            name.ThrowExceptionIfNullOrEmpty("name");
            passwordDto.Password.ThrowExceptionIfNullOrEmpty("passwordDto.Password");

            _blaiseApi.ChangePassword(name, passwordDto.Password);
        }

        public void UpdateUser(string name, UpdateUserDto userDto)
        {
            name.ThrowExceptionIfNullOrEmpty("name");
            userDto.Role.ThrowExceptionIfNullOrEmpty("userDto.Role");
            userDto.ServerParks.ThrowExceptionIfNullOrEmpty("userDto.ServerParks");

            _blaiseApi.EditUser(name, userDto.Role, userDto.ServerParks);
        }
    }
}
