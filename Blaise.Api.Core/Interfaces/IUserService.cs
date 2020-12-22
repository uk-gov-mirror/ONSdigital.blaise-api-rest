using System.Collections.Generic;
using Blaise.Api.Contracts.Models.User;

namespace Blaise.Api.Core.Interfaces
{
    public interface IUserService
    { 
        IEnumerable<UserDto> GetUsers();
        UserDto GetUser(string name);
        bool UserExists(string name);
        void AddUser(AddUserDto userDto);
        void RemoveUser(string name);
        void UpdatePassword(string name, UpdateUserPasswordDto passwordDto);
        void UpdateRole(string name, UpdateUserRoleDto roleDto);
        void UpdateServerParks(string name, UpdateUserServerParksDto serverParksDto);
    }
}