using System.Collections.Generic;
using Blaise.Api.Contracts.Models;

namespace Blaise.Api.Core.Interfaces
{
    public interface IRoleService
    {
        IEnumerable<RoleDto> GetRoles();
        void AddRoles(IEnumerable<RoleDto> roles);
    }
}