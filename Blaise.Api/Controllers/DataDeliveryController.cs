using System.Threading.Tasks;
using System.Web.Http;
using Blaise.Api.Contracts.Models.Instrument;
using Blaise.Api.Core.Interfaces.Services;
using Blaise.Api.Logging.Services;

namespace Blaise.Api.Controllers
{
    [RoutePrefix("api/v1/serverparks/{serverParkName}/instruments/{instrumentName}/deliver")]
    public class DataDeliveryController : BaseController
    {
        private readonly IDataDeliveryService _dataDeliveryService;

        public DataDeliveryController(IDataDeliveryService dataDeliveryService)
        {
            _dataDeliveryService = dataDeliveryService;
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> DeliverInstrument([FromUri] string serverParkName, [FromBody] InstrumentPackageDto instrumentPackageDto)
        {
            LoggingService.LogInfo($"Attempting to deliver instrument '{instrumentPackageDto.InstrumentFile}' on server park '{serverParkName}'");

            await _dataDeliveryService.DeliverInstrumentAsync(serverParkName, instrumentPackageDto);

            LoggingService.LogInfo($"Instrument '{instrumentPackageDto.InstrumentFile}' installed on server park '{serverParkName}'");

            return Created($"{Request.RequestUri}/{instrumentPackageDto.InstrumentName}", instrumentPackageDto);
        }
    }
}
