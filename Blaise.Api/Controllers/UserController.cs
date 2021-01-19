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
        [Route("{userName}")]
        [ResponseType(typeof(UserDto))]
        public IHttpActionResult GetUser([FromUri] string userName)
        {
            LoggingService.LogInfo($"Attempting to get user '{userName}'");

            var user = _userService.GetUser(userName);

            LoggingService.LogInfo($"Successfully got user '{userName}'");

            return Ok(user);
        }

        [HttpGet]
        [Route("{userName}/exists")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult UserExists([FromUri] string userName)
        {
            LoggingService.LogInfo($"Attempting to see if user '{userName}' exists");

            var exists = _userService.UserExists(userName);

            LoggingService.LogInfo($"User '{userName}' exists = '{exists}'");

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
        [Route("{userName}")]
        public IHttpActionResult RemoveUser([FromUri] string userName)
        {
            LoggingService.LogInfo($"Attempting to remove user '{userName}'");

            _userService.RemoveUser(userName);

            LoggingService.LogInfo($"Successfully removed user '{userName}'");

            return NoContent();
        }

        [HttpPatch]
        [Route("{userName}/password")]
        public IHttpActionResult UpdatePassword([FromUri] string userName, [FromBody] UpdateUserPasswordDto passwordDto)
        {
            LoggingService.LogInfo($"Attempting to update password for user '{userName}'");

            _userService.UpdatePassword(userName, passwordDto);

            LoggingService.LogInfo($"Successfully updated password for user '{userName}'");

            return NoContent();
        }

        [HttpPatch]
        [Route("{userName}/role")]
        public IHttpActionResult UpdateRole([FromUri] string userName, [FromBody] UpdateUserRoleDto roleDto)
        {
            LoggingService.LogInfo($"Attempting to update user '{userName}' role to '{roleDto.Role}'");

            _userService.UpdateRole(userName, roleDto);

            LoggingService.LogInfo($"Successfully updated role for user '{userName}'");

            return NoContent();
        }

        [HttpPatch]
        [Route("{userName}/serverparks")]
        public IHttpActionResult UpdateServerParks([FromUri] string userName, [FromBody] UpdateUserServerParksDto serverParksDto)
        {
            LoggingService.LogInfo($"Attempting to update server parks for user '{userName}'");

            _userService.UpdateServerParks(userName, serverParksDto);

            LoggingService.LogInfo($"Successfully updated server parks for user '{userName}'");

            return NoContent();
        }
    }
}
