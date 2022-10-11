namespace TicketManagement.EventAPI.Dto
{
    /// <summary>
    /// Class DTO for describe event seat.
    /// </summary>
    public class EventSeatDto
    {
        public int Id { get; set; }

        public int EventAreaId { get; set; }

        public int Row { get; set; }

        public int Number { get; set; }

        public EventSeatStateDto State { get; set; }
    }
}
