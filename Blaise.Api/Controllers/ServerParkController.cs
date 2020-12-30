using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Blaise.Api.Contracts.Models.ServerPark;
using Blaise.Api.Core.Interfaces.Services;
using Blaise.Api.Log.Services;

namespace Blaise.Api.Controllers
{
    //[ExceptionFilter]
    [RoutePrefix("api/v1/serverparks")]
    public class ServerParkController : ApiController
    {
        private readonly IServerParkService _serverParkService;

        public ServerParkController(IServerParkService parkService)
        {
            _serverParkService = parkService;
        }

        [HttpGet]
        [Route("")]
        [ResponseType(typeof(IEnumerable<ServerParkDto>))]
        public IHttpActionResult GetServerParks()
        {
            LogService.Info("Obtaining a list of server parks");

            var parks = _serverParkService.GetServerParks().ToList();

            LogService.Info($"Successfully received a list of server parks '{string.Join(", ", parks)}'");

            return Ok(parks);
        }

        [HttpGet]
        [Route("{serverParkName}")]
        [ResponseType(typeof(ServerParkDto))]
        public IHttpActionResult GetServerPark([FromUri] string serverParkName)
        {
            var park = _serverParkService.GetServerPark(serverParkName);

            LogService.Info($"Successfully received server park '{serverParkName}'");

            return Ok(park);
        }

        [HttpGet]
        [Route("{serverParkName}/exists")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult ServerParkExists([FromUri] string serverParkName)
        {
            var exists = _serverParkService.ServerParkExists(serverParkName);

            LogService.Info($"Successfully found server park '{serverParkName}'");

            return Ok(exists);
        }

        [HttpPost]
        [Route("{serverParkName}/machine")]
        public IHttpActionResult RegisterMachine([FromUri] string serverParkName, [FromBody] MachineDto registerMachineDto)
        {
            LogService.Info($"Attempt to register a machine '{registerMachineDto.MachineName}'");

            _serverParkService.RegisterMachineOnServerPark(serverParkName, registerMachineDto);

            LogService.Info($"Successfully registered a machine '{registerMachineDto.MachineName}'");

            return Ok();
        }
    }
}
