namespace TicketManagement.UserAPI.Dto
{
    /// <summary>
    /// Class for model with user data with token.
    /// </summary>
    public class AuthenticateResponse
    {
        public AuthenticateResponse(UserDto user, string token)
        {
            Id = user.Id;
            Surname = user.Surname;
            Name = user.Name;
            UserName = user.Email;
            Patronymic = user.Patronymic;
            Year = user.Year;
            TimeZoneId = user.TimeZoneId;
            Balance = user.Balance;
            Language = user.Language;
            Token = token;
        }

        public string Id { get; set; }

        public string UserName { get; set; }

        public string Surname { get; set; }

        public string Name { get; set; }

        public string Patronymic { get; set; }

        public int Year { get; set; }

        public string TimeZoneId { get; set; }

        public string Language { get; set; }

        public decimal Balance { get; set; }

        public string Token { get; set; }
    }
}