namespace TicketManagement.EventAPI.Dto
{
    /// <summary>
    /// Class DTO for describe event area.
    /// </summary>
    public class EventAreaDto
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        public string Description { get; set; }

        public int CoordX { get; set; }

        public int CoordY { get; set; }

        public decimal Price { get; set; }
    }
}
