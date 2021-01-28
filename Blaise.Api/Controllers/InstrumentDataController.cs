using System.Threading.Tasks;
using System.Web.Http;
using Blaise.Api.Contracts.Models.Instrument;
using Blaise.Api.Core.Interfaces.Services;
using Blaise.Api.Logging.Services;

namespace Blaise.Api.Controllers
{
    [RoutePrefix("api/v1/serverparks/{serverParkName}/instruments/{instrumentName}/data")]
    public class InstrumentDataController : BaseController
    {
        private readonly IInstrumentDataService _deliverInstrumentService;

        public InstrumentDataController(IInstrumentDataService dataDeliveryService)
        {
            _deliverInstrumentService = dataDeliveryService;
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> DeliverInstrument([FromUri] string serverParkName, [FromBody] InstrumentPackageDto instrumentPackageDto)
        {
            LoggingService.LogInfo($"Attempting to deliver instrument '{instrumentPackageDto.InstrumentFile}' on server park '{serverParkName}'");

            var bucketPath = await _deliverInstrumentService.CreateInstrumentPackageWithDataAsync(serverParkName, instrumentPackageDto);

            LoggingService.LogInfo($"Instrument '{instrumentPackageDto.InstrumentFile}' delivered with data");

            return Created($@"gs://{bucketPath}", instrumentPackageDto);
        }
    }
}
