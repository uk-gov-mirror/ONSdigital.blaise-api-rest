using System.Collections.Generic;
using System.Web.Http;
using Blaise.Api.Contracts.Models;
using Blaise.Api.Core.Interfaces;
using Blaise.Api.Filters;
using Blaise.Api.Log.Services;

namespace Blaise.Api.Controllers
{
    [ExceptionFilter]
    [RoutePrefix("api/v1/Roles")]
    public class RoleController : ApiController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetRoles()
        {
            LogService.Info("Getting a list of roles");

            var roles = _roleService.GetRoles();

            LogService.Info("Successfully got a list of roles");

            return Ok(roles);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult AddRoles([FromBody]IEnumerable<RoleDto> roles)
        {
            LogService.Info("Adding a list of roles");

            _roleService.AddRoles(roles);

            LogService.Info("Successfully added roles");

            return Ok();
        }
    }
}
