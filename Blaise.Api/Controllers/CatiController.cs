using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Blaise.Api.Contracts.Models;
using Blaise.Api.Contracts.Models.Instrument;
using Blaise.Api.Core.Interfaces;
using Blaise.Api.Filters;
using Blaise.Api.Log.Services;

namespace Blaise.Api.Controllers
{
    [ExceptionFilter]
    [RoutePrefix("api/v1/cati")]
    public class CatiController : ApiController
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
            LogService.Info("Obtaining a list of instruments from Cati");

            var instruments = _catiService.GetCatiInstruments().ToList();

            LogService.Info($"Successfully received a list of instruments from Cati '{string.Join(", ", instruments)}'");

            return Ok(instruments);
        }
    }
}
