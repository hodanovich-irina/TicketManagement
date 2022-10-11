namespace TicketManagement.VenueAPI.Settings
{
    /// <summary>
    /// Class for describe jwt token.
    /// </summary>
    public class JwtTokenSettings
    {
        public string JwtIssuer { get; set; }

        public string JwtAudience { get; set; }

        public string JwtSecretKey { get; set; }
    }
}
