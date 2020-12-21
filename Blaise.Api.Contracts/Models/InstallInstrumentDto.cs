namespace Blaise.Api.Contracts.Models
{
    public class InstallInstrumentDto
    {
        public string InstrumentName { get; set; }
        public string InstrumentFile { get; set; }

        public string BucketPath { get; set; }
    }
}
