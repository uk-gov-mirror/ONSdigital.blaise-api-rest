using System.Collections.Generic;
using Blaise.Api.Contracts.Models;
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
        void UpdatePassword(string name, UpdatePasswordDto passwordDto);
        void UpdateUser(string name, UpdateUserDto userDto);
    }
}