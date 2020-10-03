
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace Blaise.Api.Controllers
{
    [RoutePrefix("api/v1")]
    public class HealthController : ApiController
    {
        [HttpGet]
        [Route("health")]
        [ResponseType(typeof(List<string>))]
        public IHttpActionResult HealthCheck()
        {
            Console.WriteLine("Health check");

            var results = new List<string> { "Blaise OK", "DB OK"};

            return Ok(results);
        }
    }
}
