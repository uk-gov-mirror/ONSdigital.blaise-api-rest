﻿using System.Threading.Tasks;
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
        private readonly ILoggingService _loggingService;
        
        public InstrumentDataController(
            IInstrumentDataService dataDeliveryService, 
            ILoggingService loggingService)
        {
            _instrumentDataService = dataDeliveryService;
            _loggingService = loggingService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetInstrumentWithDataAsync([FromUri] string serverParkName, [FromUri] string instrumentName)
        {
            _loggingService.LogInfo($"Attempting to download instrument '{instrumentName}' on server park '{serverParkName}'");

            var instrumentFile = await _instrumentDataService.GetInstrumentPackageWithDataAsync(serverParkName, instrumentName);
            return DownloadFile(instrumentFile);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> PostInstrumentWithDataAsync([FromUri] string serverParkName, [FromUri] string instrumentName,
            [FromBody] InstrumentDataDto instrumentDataDto)
        {
            _loggingService.LogInfo($"Attempting to ingest data for instrument '{instrumentName}' on server park '{serverParkName}'");

            await _instrumentDataService.ImportOnlineDataAsync(instrumentDataDto.InstrumentDataPath, serverParkName, instrumentName);
            return Created("{Request.RequestUri}", instrumentDataDto);
        }
    }
}
