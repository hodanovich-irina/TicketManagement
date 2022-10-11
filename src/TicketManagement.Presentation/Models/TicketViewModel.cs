using System;

namespace TicketManagement.Presentation.Models
{
    /// <summary>
    /// Class for describe ticket.
    /// </summary>
    public class TicketViewModel
    {
        public int Id { get; set; }

        public decimal Price { get; set; }

        public int Row { get; set; }

        public int Number { get; set; }

        public string UserName { get; set; }

        public string EventName { get; set; }

        public DateTime EventDateStart { get; set; }

        public DateTime EventDateEnd { get; set; }

        public string UserId { get; set; }

        public decimal UserBalance { get; set; }

        public int EventSeatId { get; set; }

        public DateTime DateOfPurchase { get; set; }
    }
}
