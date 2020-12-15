
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Blaise.Api.Contracts.Enums;
using Blaise.Api.Core.Interfaces;

namespace Blaise.Api.Controllers
{
    [RoutePrefix("api/v1")]
    public class HealthController : ApiController
    {
        private readonly IHealthCheckService _healthService;

        public HealthController(IHealthCheckService healthService)
        {
            _healthService = healthService;
        }
        
        [HttpGet]
        [Route("health")]
        [ResponseType(typeof(List<string>))]
        public IHttpActionResult HealthCheck()
        {
            Console.WriteLine("performing Health check on Blaise connectivity");

            var results = _healthService.PerformCheck().ToList();

            if (results.Any(r => r.StatusType == HealthStatusType.NotOk))
            {
                return Content(HttpStatusCode.ServiceUnavailable, results);
            }

            return Ok(results);
        }
    }
}
