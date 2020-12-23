using System.Collections.Generic;

namespace Blaise.Api.Contracts.Models
{
    public class RoleDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<string> Permissions { get; set; }
    }
}
