using System.Collections.Generic;
using Blaise.Api.Contracts.Models;
using Blaise.Api.Core.Interfaces;
using Blaise.Nuget.Api.Contracts.Interfaces;

namespace Blaise.Api.Core.Services
{
    public class CatiService : ICatiService
    {
        private readonly IBlaiseSurveyApi _blaiseApi;
        private readonly ICatiInstrumentMapper _mapper;

        public CatiService(
            IBlaiseSurveyApi blaiseApi, 
            ICatiInstrumentMapper mapper)
        {
            _blaiseApi = blaiseApi;
            _mapper = mapper;
        }
        
        public List<CatiInstrumentDto> GetInstrumentsFromCati()
        {
            var instruments = _blaiseApi.GetSurveysAcrossServerParks();
            var catiInstrumentDtos = new List<CatiInstrumentDto>();

            foreach (var instrument in instruments)
            {
                var surveyDays = _blaiseApi.GetSurveyDays(instrument.Name, instrument.ServerPark);
                var catiInstrumentDto = _mapper.MapToDto(instrument, surveyDays);
                catiInstrumentDtos.Add(catiInstrumentDto);
            }

            return catiInstrumentDtos;
        }
    }
}
