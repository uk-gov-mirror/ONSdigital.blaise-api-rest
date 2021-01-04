
using System.Collections.Generic;

namespace Blaise.Api.Contracts.Models.ServerPark
{
    public class MachineDto
    {
        public string MachineName { get; set; }

        public string LogicalRootName { get; set; }

        public IList<string> Roles { get; set; }

        public int AdminPort { get; set; }
    }
}
