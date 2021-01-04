
using System.Collections.Generic;

namespace Blaise.Api.Contracts.Models.ServerPark
{
    public class MachineDto
    {
        public string MachineName { get; set; }

        public string LogicalServerName { get; set; }

        public IList<string> Roles { get; set; }
    }
}
