namespace TicketManagement.DataAccess.Models
{
    /// <summary>
    /// Class for describe venue.
    /// </summary>
    public class Venue
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Name { get; set; }
    }
}
