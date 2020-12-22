using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using Blaise.Api.Contracts.Models;
using Blaise.Api.Core.Interfaces;
using Blaise.Api.Filters;
using Blaise.Api.Log.Services;

namespace Blaise.Api.Controllers
{
    [ExceptionFilter]
    [RoutePrefix("api/v1/roles")]
    public class RoleController : ApiController
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
            LogService.Info("Getting a list of roles");

            var roles = _roleService.GetRoles();

            LogService.Info("Successfully got a list of roles");

            return Ok(roles);
        }

        [HttpGet]
        [Route("{name}")]
        [ResponseType(typeof(RoleDto))]
        public IHttpActionResult GetRole([FromUri] string name)
        {
            LogService.Info($"Attempting to get role '{name}'");

            var role = _roleService.GetRole(name);

            LogService.Info($"Successfully got role '{name}'");

            return Ok(role);
        }

        [HttpGet]
        [Route("{name}/exists")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult RoleExists([FromUri] string name)
        {
            LogService.Info($"Attempting to see if role '{name}' exists");

            var exists = _roleService.RoleExists(name);

            LogService.Info($"Role '{name}' exists = '{exists}'");

            return Ok(exists);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult AddRole([FromBody] RoleDto roleDto)
        {
            LogService.Info($"Attempting to add role '{roleDto.Name}'");
            
            _roleService.AddRole(roleDto);
            
            LogService.Info($"Successfully added role '{roleDto.Name}'");

            return Created($"{Request.RequestUri}/{roleDto.Name}", roleDto);
        }

        [HttpDelete]
        [Route("{name}")]
        public IHttpActionResult RemoveRole([FromUri] string name)
        {
            LogService.Info($"Attempting to remove role '{name}'");

            _roleService.RemoveRole(name);

            LogService.Info($"Successfully removed role '{name}'");

            return Ok();
        }

        [HttpPatch]
        [Route("{name}/permissions")]
        public IHttpActionResult UpdateRolePermissions([FromUri] string name, [FromBody] IEnumerable<string> permissions)
        {
            LogService.Info("Attempting to update permissions for role '{name}'");

            _roleService.UpdateRolePermissions(name, permissions);

            LogService.Info($"Successfully updated permissions for role '{name}'");

            return Ok();
        }
    }
}
