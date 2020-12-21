using System.Collections.Generic;

namespace Blaise.Api.Contracts.Models.User
{
    public class UserDto
    {
        public UserDto()
        {
            ServerParks = new List<string>();
        }

        public string Name { get; set; }

        public string Role { get; set; }

        public List<string> ServerParks { get; set; }
    }
}
