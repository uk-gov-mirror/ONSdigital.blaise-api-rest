using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Blaise.Api.Contracts.Models.Cati;
using Blaise.Api.Contracts.Models.Instrument;
using Blaise.Api.Core.Interfaces.Services;
using Blaise.Api.Filters;
using Blaise.Api.Logging.Services;

namespace Blaise.Api.Controllers
{
    [ExceptionFilter]
    [RoutePrefix("api/v1/cati")]
    public class CatiController : BaseController
    {
        private readonly ICatiService _catiService;

        public CatiController(ICatiService catiService)
        {
            _catiService = catiService;
        }

        [HttpGet]
        [Route("instruments")]
        [ResponseType(typeof(IEnumerable<CatiInstrumentDto>))]
        public IHttpActionResult GetInstruments()
        {
            LoggingService.LogInfo("Obtaining a list of instruments from Cati");

            var instruments = _catiService.GetCatiInstruments().ToList();

            LoggingService.LogInfo($"Successfully received a list of instruments from Cati '{string.Join(", ", instruments)}'");

            return Ok(instruments);
        }

        [HttpPost]
        [Route("serverparks/{serverParkName}/instruments/{instrumentName}/daybatch")]
        public IHttpActionResult CreateDaybatch([FromUri] string serverParkName, [FromUri] string instrumentName, [FromBody] DayBatchDto dayBatchDto)
        {
            LoggingService.LogInfo($"Create a daybatch for instrument '{instrumentName}' on server park '{serverParkName}' for '{dayBatchDto.DaybatchDate}'");

            _catiService.CreateDayBatch(instrumentName, serverParkName, dayBatchDto);

            LoggingService.LogInfo($"Daybatch created for instrument '{instrumentName}' on '{dayBatchDto.DaybatchDate}'");

            return Created("", dayBatchDto);
        }
    }
}
