using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Blaise.Api.Contracts.Models;
using Blaise.Api.Core.Interfaces;

namespace Blaise.Api.Controllers
{
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
            try
            {
                Console.WriteLine("Obtaining a list of instruments from Cati");

                var instruments = _catiService.GetCatiInstruments().ToList();

                Console.WriteLine($"Successfully received a list of instruments from Cati '{string.Join(", ", instruments)}'");

                return Ok(instruments);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}, {e.InnerException}");
                throw;
            }
        }
    }
}
