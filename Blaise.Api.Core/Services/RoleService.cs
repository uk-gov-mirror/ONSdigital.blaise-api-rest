using System.Collections.Generic;
using Blaise.Api.Contracts.Models;
using Blaise.Api.Core.Extensions;
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

        public RoleDto GetRole(string name)
        {
            name.ThrowExceptionIfNullOrEmpty("name");
            
            var role = _blaiseApi.GetRole(name);

            return _dtoMapper.MapToRoleDto(role);
        }

        public bool RoleExists(string name)
        {
            name.ThrowExceptionIfNullOrEmpty("name");

            return _blaiseApi.RoleExists(name);
        }

        public void AddRoles(IEnumerable<RoleDto> roles)
        {
            foreach (var role in roles)
            {
                _blaiseApi.AddRole(role.Name, role.Description, role.Permissions);
            }
        }

        public void AddRole(RoleDto role)
        {
            role.Name.ThrowExceptionIfNullOrEmpty("RoleDto.Name");

            _blaiseApi.AddRole(role.Name, role.Description, role.Permissions);
        }

        public void RemoveRole(string name)
        {
            name.ThrowExceptionIfNullOrEmpty("name");

            _blaiseApi.RemoveRole(name);
        }

        public void UpdateRolePermissions(string name, IEnumerable<string> permissions)
        {
            name.ThrowExceptionIfNullOrEmpty("name");

            _blaiseApi.UpdateRolePermissions(name, permissions);
        }
    }
}
