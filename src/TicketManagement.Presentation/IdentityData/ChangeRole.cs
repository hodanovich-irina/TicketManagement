using System.Collections.Generic;

namespace TicketManagement.Presentation.IdentityData
{
    /// <summary>
    /// Class for describe role.
    /// </summary>
    public class ChangeRole
    {
        public ChangeRole()
        {
            AllRoles = new List<string>();
            UserRoles = new List<string>();
        }

        public string UserId { get; set; }

        public string UserEmail { get; set; }

        public List<string> AllRoles { get; set; }

        public IList<string> UserRoles { get; set; }
    }
}
