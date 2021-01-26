using System.Collections.Generic;
using Blaise.Api.Contracts.Models.Cati;
using Blaise.Api.Core.Extensions;
using Blaise.Api.Core.Interfaces.Mappers;
using Blaise.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Contracts.Interfaces;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Api.Core.Services
{
    public class CatiService : ICatiService
    {
        private readonly IBlaiseCatiApi _blaiseApi;
        private readonly IServerParkService _serverParkService;
        private readonly IInstrumentDtoMapper _mapper;

        public CatiService(
            IBlaiseCatiApi blaiseApi,
            IServerParkService serverParkService,
            IInstrumentDtoMapper mapper)
        {
            _blaiseApi = blaiseApi;
            _serverParkService = serverParkService;
            _mapper = mapper;
        }

        public List<CatiInstrumentDto> GetCatiInstruments()
        {
            var serverParks = _serverParkService.GetServerParks();
            var catiInstruments = new List<CatiInstrumentDto>();

            foreach (var serverPark in serverParks)
            {
                var instruments = _blaiseApi.GetInstalledSurveys(serverPark.Name);
                catiInstruments.AddRange(GetCatiInstruments(instruments));
            }

            return catiInstruments;
        }

        public List<CatiInstrumentDto> GetCatiInstruments(string serverParkName)
        {
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            var instruments = _blaiseApi.GetInstalledSurveys(serverParkName);

            return GetCatiInstruments(instruments);
        }

        public CatiInstrumentDto GetCatiInstrument(string serverParkName, string instrumentName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            var instrument = _blaiseApi.GetInstalledSurvey(instrumentName, serverParkName);

            return GetCatiInstrumentDto(instrument);
        }

        public void CreateDayBatch(string instrumentName, string serverParkName, DayBatchDto dayBatchDto)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            dayBatchDto.ThrowExceptionIfNull("dayBatchDto");

            _blaiseApi.CreateDayBatch(instrumentName, serverParkName, dayBatchDto.DaybatchDate);
        }
        
        private List<CatiInstrumentDto> GetCatiInstruments(IEnumerable<ISurvey> instruments)
        {
            var catiInstruments = new List<CatiInstrumentDto>();

            foreach (var instrument in instruments)
            {
                catiInstruments.Add(GetCatiInstrumentDto(instrument));
            }

            return catiInstruments;
        }

        private CatiInstrumentDto GetCatiInstrumentDto(ISurvey instrument)
        {
            var surveyDays = _blaiseApi.GetSurveyDays(instrument.Name, instrument.ServerPark);
            return _mapper.MapToCatiInstrumentDto(instrument, surveyDays);
        }
    }
}
