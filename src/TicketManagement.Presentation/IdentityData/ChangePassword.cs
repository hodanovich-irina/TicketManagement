namespace TicketManagement.Presentation.IdentityData
{
    /// <summary>
    /// Class for describe password.
    /// </summary>
    public class ChangePassword
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string NewPassword { get; set; }

        public string OldPassword { get; set; }

        public User User { get; set; }
    }
}
