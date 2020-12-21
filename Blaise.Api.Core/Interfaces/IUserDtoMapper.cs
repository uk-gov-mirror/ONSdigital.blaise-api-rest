using System.Collections.Generic;
using Blaise.Api.Contracts.Models.User;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Api.Core.Interfaces
{
    public interface IUserDtoMapper
    {
        UserDto MapToDto(IUser user);
        IEnumerable<UserDto> MapToDtoList(IEnumerable<IUser> users);
    }
}