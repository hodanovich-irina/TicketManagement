namespace TicketManagement.Presentation.IdentityData
{
    /// <summary>
    /// Class for describe user.
    /// </summary>
    public class EditUser
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string Surname { get; set; }

        public string Name { get; set; }

        public string Patronymic { get; set; }

        public int Year { get; set; }

        public string TimeZoneId { get; set; }

        public string Language { get; set; }

        public decimal Balance { get; set; }
    }
}
