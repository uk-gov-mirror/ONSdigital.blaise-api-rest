using System.Collections.Generic;
using Blaise.Api.Contracts.Models;
using StatNeth.Blaise.API.Security;

namespace Blaise.Api.Core.Interfaces
{
    public interface IRoleDtoMapper
    {
        IEnumerable<RoleDto> MapToRoleDtos(IEnumerable<IRole> roles);
    }
}