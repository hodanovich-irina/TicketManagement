using System;
using System.Text.Json.Serialization;

namespace TicketManagement.EventAPI.Dto
{
    /// <summary>
    /// Class DTO for describe event.
    /// </summary>
    public class EventDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int LayoutId { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        public decimal BaseAreaPrice { get; set; }

        public string ImageURL { get; set; }

        public TimeSpan ShowTime { get; set; }
    }
}
