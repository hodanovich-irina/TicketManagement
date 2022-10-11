using Microsoft.AspNetCore.Identity;

namespace TicketManagement.UserAPI.Dto
{
    /// <summary>
    /// Class for describe identity user with some new properties.
    /// </summary>
    public class UserDto : IdentityUser
    {
        public string Surname { get; set; }

        public string Name { get; set; }

        public string Patronymic { get; set; }

        public int Year { get; set; }

        public string TimeZoneId { get; set; }

        public string Language { get; set; }

        public decimal Balance { get; set; }
    }
}
