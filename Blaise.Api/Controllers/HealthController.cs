
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Blaise.Api.Contracts.Enums;
using Blaise.Api.Contracts.Models;
using Blaise.Api.Core.Interfaces;
using Blaise.Api.Filters;

namespace Blaise.Api.Controllers
{
    [ExceptionFilter]
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
        [ResponseType(typeof(List<HealthCheckResultDto>))]
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
