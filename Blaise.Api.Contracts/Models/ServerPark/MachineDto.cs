
using System.Collections.Generic;

namespace Blaise.Api.Contracts.Models.ServerPark
{
    public class MachineDto
    {
        public string MachineName { get; set; }

        public string LogicalRootName { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
