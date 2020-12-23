using System.Collections.Generic;
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

        public void AddUser(AddUserDto addUserDto)
        {
            addUserDto.Name.ThrowExceptionIfNullOrEmpty("addUserDto.Name");
            addUserDto.Password.ThrowExceptionIfNullOrEmpty("addUserDto.Password");
            addUserDto.Role.ThrowExceptionIfNullOrEmpty("addUserDto.Role");
            addUserDto.ServerParks.ThrowExceptionIfNullOrEmpty("addUserDto.ServerParks");

            _blaiseApi.AddUser(addUserDto.Name, addUserDto.Password, addUserDto.Role, addUserDto.ServerParks, addUserDto.DefaultServerPark);
        }

        public void UpdatePassword(string name, UpdateUserPasswordDto updateUserPasswordDto)
        {
            name.ThrowExceptionIfNullOrEmpty("name");
            updateUserPasswordDto.Password.ThrowExceptionIfNullOrEmpty("updateUserPasswordDto.Password");

            _blaiseApi.UpdatePassword(name, updateUserPasswordDto.Password);
        }

        public void UpdateRole(string name, UpdateUserRoleDto updateUserRoleDto)
        {
            name.ThrowExceptionIfNullOrEmpty("name");
            updateUserRoleDto.Role.ThrowExceptionIfNullOrEmpty("updateUserRoleDto.Role");

            _blaiseApi.UpdateRole(name, updateUserRoleDto.Role);
        }

        public void UpdateServerParks(string name, UpdateUserServerParksDto updateUserServerParksDto)
        {
            name.ThrowExceptionIfNullOrEmpty("name");
            updateUserServerParksDto.ServerParks.ThrowExceptionIfNullOrEmpty("updateUserServerParksDto.ServerParks");

            _blaiseApi.UpdateServerParks(name, updateUserServerParksDto.ServerParks, updateUserServerParksDto.DefaultServerPark);
        }
        public void RemoveUser(string name)
        {
            name.ThrowExceptionIfNullOrEmpty("name");

            _blaiseApi.RemoveUser(name);
        }
    }
}
