using System.Collections.Generic;

namespace TicketManagement.UserAPI.Dto
{
    /// <summary>
    /// Class for describe user with roles.
    /// </summary>
    public class UserRolesModel
    {
        public UserDto User { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
