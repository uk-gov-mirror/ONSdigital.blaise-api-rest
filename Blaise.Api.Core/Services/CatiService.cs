using System.Collections.Generic;
using Blaise.Api.Contracts.Models;
using Blaise.Api.Contracts.Models.Instrument;
using Blaise.Api.Core.Interfaces;
using Blaise.Nuget.Api.Contracts.Interfaces;

namespace Blaise.Api.Core.Services
{
    public class CatiService : ICatiService
    {
        private readonly IBlaiseCatiApi _blaiseApi;
        private readonly IInstrumentService _instrumentService;
        private readonly ICatiInstrumentDtoMapper _mapper;

        public CatiService(
            IBlaiseCatiApi blaiseApi,
            IInstrumentService instrumentService,
            ICatiInstrumentDtoMapper mapper)
        {
            _blaiseApi = blaiseApi;
            _instrumentService = instrumentService;
            _mapper = mapper;
        }
        
        public List<CatiInstrumentDto> GetCatiInstruments()
        {
            var instruments = _instrumentService.GetAllInstruments();
            var catiInstruments = new List<CatiInstrumentDto>();

            foreach (var instrument in instruments)
            {
                var surveyDays = _blaiseApi.GetSurveyDays(instrument.Name, instrument.ServerParkName);
                var catiInstrument = _mapper.MapToCatiInstrumentDto(instrument, surveyDays);
                catiInstruments.Add(catiInstrument);
            }

            return catiInstruments;
        }
    }
}
