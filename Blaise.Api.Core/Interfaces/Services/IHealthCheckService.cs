using System.Collections.Generic;
using Blaise.Api.Contracts.Models;

namespace Blaise.Api.Core.Interfaces.Services
{
    public interface IHealthCheckService
    {
        IEnumerable<HealthCheckResultDto> PerformCheck();
    }
}