using System.Collections.Generic;

namespace Blaise.Api.Core.Interfaces
{
    public interface IParkService
    {
        IEnumerable<string> GetParks();
    }
}