using System.Collections.Generic;
using System.Linq;
using Blaise.Api.Contracts.Models.Role;
using Blaise.Api.Core.Interfaces.Mappers;
using StatNeth.Blaise.API.Security;

namespace Blaise.Api.Core.Mappers
{
    public class RoleDtoMapper : IRoleDtoMapper
    {
        public IEnumerable<RoleDto> MapToRoleDtos(IEnumerable<IRole> roles)
        {
            var roleDtos = new List<RoleDto>();

            foreach (var role in roles)
            {
                roleDtos.Add(MapToRoleDto(role));
            }

            return roleDtos;
        }

        public RoleDto MapToRoleDto(IRole role)
        {
            return new RoleDto
            {
                Name = role.Name,
                Description = role.Description,
                Permissions = role.Permissions
                    .Where(p => p.Permission == PermissionStatus.Allowed)
                    .Select(p => p.Action)
            };
        }
    }
}
