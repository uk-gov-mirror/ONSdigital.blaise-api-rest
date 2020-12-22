using System.Collections.Generic;
using System.Linq;

namespace Blaise.Api.Contracts.Models.User
{
    public class UserServerParksDto
    {
        public UserServerParksDto()
        {
            ServerParks = new List<string>();
        }

        public List<string> ServerParks { get; set; }

        public string DefaultServerPark => ServerParks.FirstOrDefault();
    }
}
