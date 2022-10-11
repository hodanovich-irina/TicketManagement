using TicketManagement.Presentation.Dto;

namespace TicketManagement.Presentation.Models
{
    /// <summary>
    /// Class for describe event seat.
    /// </summary>
    public class EventSeatViewModel
    {
        public int Id { get; set; }

        public int EventAreaId { get; set; }

        public string EventName { get; set; }

        public int AreaX { get; set; }

        public int AreaY { get; set; }

        public int Row { get; set; }

        public int Number { get; set; }

        public EventSeatStateDto State { get; set; }
    }
}
