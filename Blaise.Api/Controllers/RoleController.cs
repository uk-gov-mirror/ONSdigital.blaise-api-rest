using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using Blaise.Api.Contracts.Models.Role;
using Blaise.Api.Core.Interfaces.Services;
using Blaise.Api.Filters;
using Blaise.Api.Logging.Services;

namespace Blaise.Api.Controllers
{
    [ExceptionFilter]
    [RoutePrefix("api/v1/roles")]
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [Route("")]
        [ResponseType(typeof(IEnumerable<RoleDto>))]
        public IHttpActionResult GetRoles()
        {
            LoggingService.LogInfo("Getting a list of roles");

            var roles = _roleService.GetRoles();

            LoggingService.LogInfo("Successfully got a list of roles");

            return Ok(roles);
        }

        [HttpGet]
        [Route("{name}")]
        [ResponseType(typeof(RoleDto))]
        public IHttpActionResult GetRole([FromUri] string name)
        {
            LoggingService.LogInfo($"Attempting to get role '{name}'");

            var role = _roleService.GetRole(name);

            LoggingService.LogInfo($"Successfully got role '{name}'");

            return Ok(role);
        }

        [HttpGet]
        [Route("{name}/exists")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult RoleExists([FromUri] string name)
        {
            LoggingService.LogInfo($"Attempting to see if role '{name}' exists");

            var exists = _roleService.RoleExists(name);

            LoggingService.LogInfo($"Role '{name}' exists = '{exists}'");

            return Ok(exists);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult AddRole([FromBody] RoleDto roleDto)
        {
            LoggingService.LogInfo($"Attempting to add role '{roleDto.Name}'");
            
            _roleService.AddRole(roleDto);
            
            LoggingService.LogInfo($"Successfully added role '{roleDto.Name}'");

            return Created($"{Request.RequestUri}/{roleDto.Name}", roleDto);
        }

        [HttpDelete]
        [Route("{name}")]
        public IHttpActionResult RemoveRole([FromUri] string name)
        {
            LoggingService.LogInfo($"Attempting to remove role '{name}'");

            _roleService.RemoveRole(name);

            LoggingService.LogInfo($"Successfully removed role '{name}'");

            return NoContent();
        }

        [HttpPatch]
        [Route("{name}/permissions")]
        public IHttpActionResult UpdateRolePermissions([FromUri] string name, [FromBody] IEnumerable<string> permissions)
        {
            LoggingService.LogInfo("Attempting to update permissions for role '{name}'");

            _roleService.UpdateRolePermissions(name, permissions);

            LoggingService.LogInfo($"Successfully updated permissions for role '{name}'");

            return NoContent();
        }
    }
}
