using System.Collections.Generic;
using TicketManagement.Presentation.Dto;

namespace TicketManagement.Presentation.Models
{
    /// <summary>
    /// Class for describe event area.
    /// </summary>
    public class EventAreaViewModel
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        public string EventName { get; set; }

        public List<EventDto> Events { get; set; }

        public string Description { get; set; }

        public int CoordX { get; set; }

        public int CoordY { get; set; }

        public decimal Price { get; set; }
    }
}
