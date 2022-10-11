using System;

namespace TicketManagement.Presentation.Models
{
    /// <summary>
    /// Class for describe event.
    /// </summary>
    public class EventViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string LayoutName { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        public decimal BaseAreaPrice { get; set; }

        public string ImageURL { get; set; }

        public TimeSpan ShowTime { get; set; }

        public string VenueName { get; set; }
    }
}
