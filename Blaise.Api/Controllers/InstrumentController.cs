using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Blaise.Api.Contracts.Models;
using Blaise.Api.Core.Interfaces;
using Blaise.Api.Filters;

namespace Blaise.Api.Controllers
{
    [ExceptionFilter]
    [RoutePrefix("api/v1/serverparks/{serverParkName}")]
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
        [Route("instruments")]
        [ResponseType(typeof(IEnumerable<InstrumentDto>))]
        public IHttpActionResult GetInstruments(string serverParkName)
        {
            Console.WriteLine("Obtaining a list of instruments for a server park");

            var instruments = _instrumentService.GetInstruments(serverParkName).ToList();

            Console.WriteLine($"Successfully received a list of instruments '{string.Join(", ", instruments)}'");

            return Ok(instruments);
        }

        [HttpGet]
        [Route("instruments/{instrumentName}")]
        [ResponseType(typeof(InstrumentDto))]
        public IHttpActionResult GetInstrument(string serverParkName, string instrumentName)
        {
            Console.WriteLine("Obtaining an instruments for a server park");

            var instruments = _instrumentService
                .GetInstrument(instrumentName, serverParkName);

            Console.WriteLine($"Successfully received an instrument '{instrumentName}'");

            return Ok(instruments);
        }

        [HttpGet]
        [Route("instruments/{instrumentName}/exists")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult InstrumentExists(string serverParkName, string instrumentName)
        {
            Console.WriteLine($"Check that an instrument exists on server park '{serverParkName}'");

            var exists = _instrumentService.InstrumentExists(instrumentName, serverParkName);

            Console.WriteLine($"Instrument '{instrumentName}' exists = '{exists}' on '{serverParkName}'");

            return Ok(exists);
        }

        [HttpPost]
        [Route("install")]
        public IHttpActionResult InstallInstrument([FromUri]string serverParkName, [FromBody] InstallInstrumentDto installInstrumentDto)
        {
            Console.WriteLine($"Attempting to install instrument '{installInstrumentDto.InstrumentFile}' on server park '{serverParkName}'");

           _installInstrumentService.InstallInstrument(installInstrumentDto.BucketPath, 
               installInstrumentDto.InstrumentFile, serverParkName);

            Console.WriteLine($"Instrument '{installInstrumentDto.InstrumentFile}' installed on server park '{serverParkName}'");

            return Ok();
        }
    }
}
