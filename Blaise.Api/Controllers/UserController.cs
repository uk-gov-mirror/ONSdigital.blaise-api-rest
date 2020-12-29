using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using Blaise.Api.Contracts.Models.User;
using Blaise.Api.Core.Interfaces.Services;
using Blaise.Api.Filters;
using Blaise.Api.Logging.Services;

namespace Blaise.Api.Controllers
{
    [ExceptionFilter]
    [RoutePrefix("api/v1/users")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("")]
        [ResponseType(typeof(IEnumerable<UserDto>))]
        public IHttpActionResult GetUsers()
        {
            LoggingService.LogInfo("Getting a list of users");

            var users = _userService.GetUsers();

            LoggingService.LogInfo("Successfully got a list of users");

            return Ok(users);
        }

        [HttpGet]
        [Route("{name}")]
        [ResponseType(typeof(UserDto))]
        public IHttpActionResult GetUser([FromUri] string name)
        {
            LoggingService.LogInfo($"Attempting to get user '{name}'");

            var user = _userService.GetUser(name);

            LoggingService.LogInfo($"Successfully got user '{name}'");

            return Ok(user);
        }

        [HttpGet]
        [Route("{name}/exists")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult UserExists([FromUri] string name)
        {
            LoggingService.LogInfo($"Attempting to see if user '{name}' exists");

            var exists = _userService.UserExists(name);

            LoggingService.LogInfo($"User '{name}' exists = '{exists}'");

            return Ok(exists);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult AddUser([FromBody] AddUserDto userDto)
        {
            LoggingService.LogInfo($"Attempting to add user '{userDto.Name}'");

            _userService.AddUser(userDto);

            LoggingService.LogInfo($"Successfully added role '{userDto.Name}'");

            return Created($"{Request.RequestUri}/{userDto.Name}", userDto);
        }

        [HttpDelete]
        [Route("{name}")]
        public IHttpActionResult RemoveUser([FromUri] string name)
        {
            LoggingService.LogInfo($"Attempting to remove user '{name}'");

            _userService.RemoveUser(name);

            LoggingService.LogInfo($"Successfully removed user '{name}'");

            return NoContent();
        }

        [HttpPatch]
        [Route("{name}/password")]
        public IHttpActionResult UpdatePassword([FromUri] string name, [FromBody] UpdateUserPasswordDto passwordDto)
        {
            LoggingService.LogInfo($"Attempting to update password for user '{name}'");

            _userService.UpdatePassword(name, passwordDto);

            LoggingService.LogInfo($"Successfully updated password for user '{name}'");

            return NoContent();
        }

        [HttpPatch]
        [Route("{name}/role")]
        public IHttpActionResult UpdateRole([FromUri] string name, [FromBody] UpdateUserRoleDto roleDto)
        {
            LoggingService.LogInfo($"Attempting to update user '{name}' role to '{roleDto.Role}'");

            _userService.UpdateRole(name, roleDto);

            LoggingService.LogInfo($"Successfully updated role for user '{name}'");

            return NoContent();
        }

        [HttpPatch]
        [Route("{name}/serverparks")]
        public IHttpActionResult UpdateServerParks([FromUri] string name, [FromBody] UpdateUserServerParksDto serverParksDto)
        {
            LoggingService.LogInfo($"Attempting to update server parks for user '{name}'");

            _userService.UpdateServerParks(name, serverParksDto);

            LoggingService.LogInfo($"Successfully updated server parks for user '{name}'");

            return NoContent();
        }
    }
}
