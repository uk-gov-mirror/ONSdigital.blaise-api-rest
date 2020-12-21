using System.Collections.Generic;

namespace Blaise.Api.Contracts.Models.User
{
    public class UpdateUserDto
    {
        public UpdateUserDto()
        {
            ServerParks = new List<string>();
        }

        public string Role { get; set; }

        public List<string> ServerParks { get; set; }
    }
}
