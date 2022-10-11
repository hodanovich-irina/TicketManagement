using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Presentation.IdentityData
{
    /// <summary>
    /// Class for describe login.
    /// </summary>
    public class Login
    {
        public string Email { get; set; }

        [Required(ErrorMessage = "PasswordError")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}
