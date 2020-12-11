﻿using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Api.Contracts.Models;
using Blaise.Api.Core.Interfaces;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Api.Core.Mappers
{
    public class CatiInstrumentMapper : ICatiInstrumentMapper
    {
        public CatiInstrumentDto MapToCatiInstrumentDto(InstrumentDto instrumentDto, List<DateTime> surveyDays)
        {
            var catiInstrument = new CatiInstrumentDto
            {
                Name = instrumentDto.Name,
                ServerParkName = instrumentDto.ServerParkName,
                InstallDate = instrumentDto.InstallDate,
                SurveyDays = surveyDays,
                Expired = surveyDays.All(s => s.Date < DateTime.Today),
                ActiveToday = surveyDays.Any(s => s.Date == DateTime.Today)
            };

            return catiInstrument;
        }

    }
}
