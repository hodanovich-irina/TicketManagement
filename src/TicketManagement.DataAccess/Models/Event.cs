using System;

namespace TicketManagement.DataAccess.Models
{
    /// <summary>
    /// Class for describe event.
    /// </summary>
    public class Event
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
