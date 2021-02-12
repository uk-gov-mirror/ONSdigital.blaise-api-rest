using System.Threading.Tasks;
using System.Web.Http;
using Blaise.Api.Contracts.Interfaces;
using Blaise.Api.Contracts.Models.Instrument;
using Blaise.Api.Core.Interfaces.Services;

namespace Blaise.Api.Controllers
{
    [RoutePrefix("api/v1/serverparks/{serverParkName}/instruments/{instrumentName}/data")]
    public class InstrumentDataController : BaseController
    {
        private readonly IInstrumentDataService _instrumentDataService;
  
        public InstrumentDataController(
            IInstrumentDataService dataDeliveryService, 
            ILoggingService loggingService) : base(loggingService)
        {
            _instrumentDataService = dataDeliveryService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetInstrumentWithDataAsync([FromUri] string serverParkName, [FromUri] string instrumentName)
        {
            var instrumentFile = await _instrumentDataService.GetInstrumentPackageWithDataAsync(serverParkName, instrumentName);

            return DownloadFile(instrumentFile);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> PostInstrumentWithDataAsync([FromUri] string serverParkName, [FromUri] string instrumentName,
            [FromBody] InstrumentDataDto instrumentDataDto)
        {
            await _instrumentDataService.ImportOnlineDataAsync(instrumentDataDto, serverParkName, instrumentName);
            return Created("{Request.RequestUri}", instrumentDataDto);
        }
    }
}
