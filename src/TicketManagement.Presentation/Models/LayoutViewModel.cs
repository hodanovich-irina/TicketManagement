namespace TicketManagement.Presentation.Models
{
    /// <summary>
    /// Class for describe layout.
    /// </summary>
    public class LayoutViewModel
    {
        public int Id { get; set; }

        public int VenueId { get; set; }

        public string VenueName { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }
    }
}
