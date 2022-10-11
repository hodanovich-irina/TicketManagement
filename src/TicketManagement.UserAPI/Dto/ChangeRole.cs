using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TicketManagement.UserAPI.Dto
{
    /// <summary>
    /// Class for describe role.
    /// </summary>
    public class ChangeRole
    {
        public ChangeRole()
        {
            AllRoles = new List<IdentityRole>();
            UserRoles = new List<string>();
        }

        public string UserId { get; set; }

        public string UserEmail { get; set; }

        public List<IdentityRole> AllRoles { get; set; }

        public IList<string> UserRoles { get; set; }
    }
}
