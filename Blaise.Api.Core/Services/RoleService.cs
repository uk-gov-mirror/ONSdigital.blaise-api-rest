using System.Collections.Generic;
using Blaise.Api.Contracts.Models;
using Blaise.Api.Core.Interfaces;
using Blaise.Nuget.Api.Contracts.Interfaces;

namespace Blaise.Api.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly IBlaiseRoleApi _blaiseApi;
        private readonly IRoleDtoMapper _dtoMapper;

        public RoleService(
            IBlaiseRoleApi blaiseApi, 
            IRoleDtoMapper dtoMapper)
        {
            _blaiseApi = blaiseApi;
            _dtoMapper = dtoMapper;
        }

        public IEnumerable<RoleDto> GetRoles()
        {
            var roles = _blaiseApi.GetRoles();

            return _dtoMapper.MapToRoleDtos(roles);

        }
        public void AddRoles(IEnumerable<RoleDto> roles)
        {
            foreach (var role in roles)
            {
                _blaiseApi.AddRole(role.Name, role.Description, role.Permissions);
            }
        }
    }
}
