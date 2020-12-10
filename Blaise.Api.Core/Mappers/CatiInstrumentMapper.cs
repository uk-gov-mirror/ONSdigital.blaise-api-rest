using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Api.Contracts.Models;
using Blaise.Api.Core.Interfaces;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Api.Core.Mappers
{
    public class CatiInstrumentMapper : ICatiInstrumentMapper
    {
        private readonly IInstrumentDtoMapper _instrumentDtoMapper;

        public CatiInstrumentMapper(IInstrumentDtoMapper instrumentDtoMapper)
        {
            _instrumentDtoMapper = instrumentDtoMapper;
        }
        public CatiInstrumentDto MapToDto(ISurvey instrument, List<DateTime> surveyDays)
        {
            var instrumentDto = _instrumentDtoMapper.MapToDto(instrument);

            return MapToDto((CatiInstrumentDto) instrumentDto, surveyDays);
        }

        private static CatiInstrumentDto MapToDto(CatiInstrumentDto instrumentDto, 
            List<DateTime> surveyDays)
        {
            instrumentDto.SurveyDays = surveyDays;
            instrumentDto.Expired = surveyDays.All(s => s.Date < DateTime.Today);
            instrumentDto.ActiveToday = surveyDays.Any(s => s.Date == DateTime.Today);
            
            return instrumentDto;
        }

    }
}
