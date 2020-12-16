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

        public InstrumentController(IInstrumentService instrumentService)
        {
            _instrumentService = instrumentService;
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
            Console.WriteLine("Check that an instrument exists on a server park");

            var exists = _instrumentService.InstrumentExists(instrumentName, serverParkName);

            Console.WriteLine($"Instrument '{instrumentName}' exists = {exists}");

            return Ok(exists);
        }
    }
}
