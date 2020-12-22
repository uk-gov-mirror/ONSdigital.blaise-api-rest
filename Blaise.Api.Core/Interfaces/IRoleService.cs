using System.Collections.Generic;
using Blaise.Api.Contracts.Models;

namespace Blaise.Api.Core.Interfaces
{
    public interface IRoleService
    {
        IEnumerable<RoleDto> GetRoles();

        RoleDto GetRole(string name);

        bool RoleExists(string name);

        void AddRole(RoleDto role);

        void RemoveRole(string name);

        void UpdateRolePermissions(string name, IEnumerable<string> permissions);
    }
}