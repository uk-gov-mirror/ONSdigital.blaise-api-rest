using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using Blaise.Api.Contracts.Models.User;
using Blaise.Api.Core.Interfaces;
using Blaise.Api.Filters;
using Blaise.Api.Log.Services;

namespace Blaise.Api.Controllers
{
    [ExceptionFilter]
    [RoutePrefix("api/v1/Users")]
    public class UserController : ApiController
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
            LogService.Info("Getting a list of users");

            var users = _userService.GetUsers();

            LogService.Info("Successfully got a list of users");

            return Ok(users);
        }

        [HttpGet]
        [Route("{name}")]
        [ResponseType(typeof(UserDto))]
        public IHttpActionResult GetUser([FromUri] string name)
        {
            LogService.Info($"Attempting to get user '{name}'");

            var user = _userService.GetUser(name);

            LogService.Info($"Successfully got user '{name}'");

            return Ok(user);
        }

        [HttpGet]
        [Route("{name}/exists")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult UserExists([FromUri] string name)
        {
            LogService.Info($"Attempting to see if user '{name}' exists");

            var exists = _userService.UserExists(name);

            LogService.Info($"User '{name}' exists = '{exists}'");

            return Ok(exists);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult AddUser([FromBody] AddUserDto userDto)
        {
            LogService.Info($"Attempting to add user '{userDto.Name}'");
            
            _userService.AddUser(userDto);
            
            LogService.Info($"Successfully added role '{userDto.Name}'");

               return Created($"{Request.RequestUri}/{userDto.Name}", userDto);
        }

        [HttpDelete]
        [Route("{name}")]
        public IHttpActionResult RemoveUser([FromUri] string name)
        {
            LogService.Info($"Attempting to remove user '{name}'");

            _userService.RemoveUser(name);

            LogService.Info($"Successfully removed user '{name}'");

            return Ok();
        }

        [HttpPatch]
        [Route("{name}/password")]
        public IHttpActionResult ChangePassword([FromUri] string name, [FromBody] UpdatePasswordDto passwordDto)
        {
            LogService.Info($"Attempting to update password for user '{name}'");

            _userService.UpdatePassword(name, passwordDto);

            LogService.Info($"Successfully updated password for user '{name}'");

            return Ok();
        }

        [HttpPatch]
        [Route("{name}")]
        public IHttpActionResult UpdateUser([FromUri] string name, [FromBody] UpdateUserDto userDto)
        {
            LogService.Info($"Attempting to update user '{name}'");

            _userService.UpdateUser(name, userDto);

            LogService.Info($"Successfully updated user '{name}'");

            return Ok();
        }
    }
}
