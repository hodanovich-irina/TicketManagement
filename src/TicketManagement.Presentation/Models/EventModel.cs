using System;

namespace TicketManagement.Presentation.Models
{
    public class EventModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int LayoutId { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        public decimal BaseAreaPrice { get; set; }

        public string ImageURL { get; set; }

        public int Minutes { get; set; }

        public int Seconds { get; set; }

        public int Hours { get; set; }
    }
}
