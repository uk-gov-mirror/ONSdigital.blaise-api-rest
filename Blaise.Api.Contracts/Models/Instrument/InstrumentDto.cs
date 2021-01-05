using System;

namespace Blaise.Api.Contracts.Models.Instrument
{
    public class InstrumentDto
    {
        public string Name { get; set; }
        public string ServerParkName { get; set; }
        public DateTime InstallDate { get; set; }
        public string Status { get; set; }

        public int DataRecordCount { get; set; }

        public bool HasData => DataRecordCount > 0;
    }
}
