namespace TicketManagement.DataAccess.Models
{
    /// <summary>
    /// Class for describe layout.
    /// </summary>
    public class Layout
    {
        public int Id { get; set; }

        public int VenueId { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }
    }
}
