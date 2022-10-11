using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Presentation.IdentityData
{
    /// <summary>
    /// Class for describe registration for user.
    /// </summary>
    public class Register
    {
        [EmailAddress]
        public string Email { get; set; }

        public int Year { get; set; }

        public string Surname { get; set; }

        public string Name { get; set; }

        public string Patronymic { get; set; }

        public string TimeZoneId { get; set; }

        public string Language { get; set; }

        [Required(ErrorMessage = "PasswordError")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "PasswordError", MinimumLength = 5)]
        public string Password { get; set; }

        [Required(ErrorMessage = "PasswordConfirmError")]
        [Compare("Password", ErrorMessage = "PasswordConfirmError")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}
