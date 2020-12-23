namespace Blaise.Api.Contracts.Models.User
{
    public class UserDto : UserServerParksDto
    {
        public string Name { get; set; }

        public string Role { get; set; }
    }
}
