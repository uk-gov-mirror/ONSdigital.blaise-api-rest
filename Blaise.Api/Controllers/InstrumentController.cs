using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Blaise.Api.Contracts.Models.Instrument;
using Blaise.Api.Core.Interfaces;
using Blaise.Api.Filters;
using Blaise.Api.Log.Services;

namespace Blaise.Api.Controllers
{
    [ExceptionFilter]
    [RoutePrefix("api/v1/serverparks/{serverParkName}/instruments")]
    public class InstrumentController : ApiController
    {
        private readonly IInstrumentService _instrumentService;
        private readonly IInstallInstrumentService _installInstrumentService;

        public InstrumentController(
            IInstrumentService instrumentService,
            IInstallInstrumentService installInstrumentService)
        {
            _instrumentService = instrumentService;
            _installInstrumentService = installInstrumentService;
        }

        [HttpGet]
        [Route("")]
        [ResponseType(typeof(IEnumerable<InstrumentDto>))]
        public IHttpActionResult GetInstruments(string serverParkName)
        {
            LogService.Info("Obtaining a list of instruments for a server park");

            var instruments = _instrumentService.GetInstruments(serverParkName).ToList();

            LogService.Info($"Successfully received a list of instruments '{string.Join(", ", instruments)}'");

            return Ok(instruments);
        }

        [HttpGet]
        [Route("{instrumentName}")]
        [ResponseType(typeof(InstrumentDto))]
        public IHttpActionResult GetInstrument([FromUri] string serverParkName, [FromUri] string instrumentName)
        {
            LogService.Info("Obtaining an instruments for a server park");

            var instruments = _instrumentService
                .GetInstrument(instrumentName, serverParkName);

            LogService.Info($"Successfully received an instrument '{instrumentName}'");

            return Ok(instruments);
        }

        [HttpGet]
        [Route("{instrumentName}/exists")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult InstrumentExists([FromUri] string serverParkName, [FromUri] string instrumentName)
        {
            LogService.Info($"Check that an instrument exists on server park '{serverParkName}'");

            var exists = _instrumentService.InstrumentExists(instrumentName, serverParkName);

            LogService.Info($"Instrument '{instrumentName}' exists = '{exists}' on '{serverParkName}'");

            return Ok(exists);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult InstallInstrument([FromUri] string serverParkName, [FromBody] InstallInstrumentDto installInstrumentDto)
        {
            LogService.Info($"Attempting to install instrument '{installInstrumentDto.InstrumentFile}' on server park '{serverParkName}'");

            _installInstrumentService.InstallInstrument(serverParkName, installInstrumentDto);

            LogService.Info($"Instrument '{installInstrumentDto.InstrumentFile}' installed on server park '{serverParkName}'");

            return Created($"{Request.RequestUri}/{installInstrumentDto.InstrumentName}", installInstrumentDto);
        }

        [HttpDelete]
        [Route("{instrumentName}")]
        public IHttpActionResult UninstallInstrument([FromUri] string serverParkName,[FromUri] string name)
        {
            LogService.Info($"Attempting to uninstall instrument '{name}' on server park '{serverParkName}'");

            _installInstrumentService.UninstallInstrument(name, serverParkName);

            LogService.Info($"Instrument '{name}' has been uninstalled from server park '{serverParkName}'");

            return Ok();
        }
    }
}
