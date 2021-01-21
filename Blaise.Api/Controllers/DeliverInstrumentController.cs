using System.Threading.Tasks;
using System.Web.Http;
using Blaise.Api.Contracts.Models.Instrument;
using Blaise.Api.Core.Interfaces.Services;
using Blaise.Api.Logging.Services;

namespace Blaise.Api.Controllers
{
    [RoutePrefix("api/v1/serverparks/{serverParkName}/instruments/{instrumentName}/deliver")]
    public class DeliverInstrumentController : BaseController
    {
        private readonly IDeliverInstrumentService _deliverInstrumentService;

        public DeliverInstrumentController(IDeliverInstrumentService dataDeliveryService)
        {
            _deliverInstrumentService = dataDeliveryService;
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> DeliverInstrument([FromUri] string serverParkName, [FromBody] InstrumentPackageDto instrumentPackageDto)
        {
            LoggingService.LogInfo($"Attempting to deliver instrument '{instrumentPackageDto.InstrumentFile}' on server park '{serverParkName}'");

            await _deliverInstrumentService.DeliverInstrumentWithDataAsync(serverParkName, instrumentPackageDto);

            LoggingService.LogInfo($"Instrument '{instrumentPackageDto.InstrumentFile}' installed on server park '{serverParkName}'");

            return Created($"{Request.RequestUri}/{instrumentPackageDto.InstrumentName}", instrumentPackageDto);
        }
    }
}
