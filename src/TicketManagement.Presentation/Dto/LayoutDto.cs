namespace TicketManagement.Presentation.Dto
{
    /// <summary>
    /// Class DTO for describe layout.
    /// </summary>
    public class LayoutDto
    {
        public int Id { get; set; }

        public int VenueId { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }
    }
}
