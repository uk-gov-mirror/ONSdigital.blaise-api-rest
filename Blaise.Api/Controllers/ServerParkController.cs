using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Blaise.Api.Contracts.Models;
using Blaise.Api.Core.Interfaces;
using Blaise.Api.Filters;

namespace Blaise.Api.Controllers
{
    [ExceptionFilter]
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
            Console.WriteLine("Obtaining a list of server parks");

            var parks = _serverParkService.GetServerParks().ToList();

            Console.WriteLine($"Successfully received a list of server parks '{string.Join(", ", parks)}'");

            return Ok(parks);
        }

        [HttpGet]
        [Route("{serverParkName}")]
        [ResponseType(typeof(ServerParkDto))]
        public IHttpActionResult GetServerPark(string serverParkName)
        {
            var park = _serverParkService.GetServerPark(serverParkName);

            Console.WriteLine($"Successfully received server park '{serverParkName}'");

            return Ok(park);
        }

        [HttpGet]
        [Route("{serverParkName}/exists")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult ServerParkExists(string serverParkName)
        {
            var exists = _serverParkService.ServerParkExists(serverParkName);

            Console.WriteLine($"Successfully found server park '{serverParkName}'");

            return Ok(exists);
        }
    }
}
