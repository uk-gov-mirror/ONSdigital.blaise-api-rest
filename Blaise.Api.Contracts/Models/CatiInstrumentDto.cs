using System;
using System.Collections.Generic;

namespace Blaise.Api.Contracts.Models
{
    public class CatiInstrumentDto : InstrumentDto
    {
        public CatiInstrumentDto()
        {
            SurveyDays = new List<DateTime>();
        }

        public List<DateTime> SurveyDays;

        public bool Expired;

        public bool ActiveToday;
    }
}