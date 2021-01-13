namespace Blaise.Api.Contracts.Interfaces
{
    public interface IConfigurationProvider
    {
        string BaseUrl { get; }
        string TempPath { get; }
    }
}