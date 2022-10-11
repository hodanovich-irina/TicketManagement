using System.Collections.Generic;

namespace TicketManagement.Presentation.IdentityData
{
    public class UserRolesModel
    {
        public User User { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
