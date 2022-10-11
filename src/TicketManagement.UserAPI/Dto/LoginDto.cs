using System.ComponentModel.DataAnnotations;

namespace TicketManagement.UserAPI.Dto
{
    /// <summary>
    /// Class for describe model for login.
    /// </summary>
    public class LoginDto
    {
        public string Email { get; set; }

        [Required(ErrorMessage = "PasswordError")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}
