using System.Collections.Generic;
using Blaise.Api.Contracts.Models;
using Blaise.Api.Contracts.Models.Role;
using StatNeth.Blaise.API.Security;

namespace Blaise.Api.Core.Interfaces.Mappers
{
    public interface IRoleDtoMapper
    {
        IEnumerable<RoleDto> MapToRoleDtos(IEnumerable<IRole> roles);
        RoleDto MapToRoleDto(IRole role);
    }
}