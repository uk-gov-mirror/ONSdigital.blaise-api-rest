using System.Web.Http;
using System.Web.Http.Description;
using Blaise.Api.Core.Interfaces.Services;
using Blaise.Api.Filters;
using Blaise.Api.Logging.Services;

namespace Blaise.Api.Controllers
{
    [ExceptionFilter]
    [RoutePrefix("api/v1/api/v1/serverparks/{serverParkName}/instruments/{instrumentName}")]
    public class CaseController : BaseController
    {
        private readonly ICaseService _caseService;

        public CaseController(ICaseService caseService)
        {
            _caseService = caseService;
        }

        [HttpGet]
        [Route("numberofcases")]
        [ResponseType(typeof(int))]
        public IHttpActionResult GetNumberOfCases([FromUri] string serverParkName, [FromUri] string instrumentName)
        {
            LoggingService.LogInfo($"Obtaining number of cases for instrument {instrumentName}");

            var numberOfCases = _caseService.GetNumberOfCases(instrumentName, serverParkName);

            LoggingService.LogInfo($"Successfully received '{numberOfCases}' cases for instrument {instrumentName}");

            return Ok(numberOfCases);
        }
    }
}
