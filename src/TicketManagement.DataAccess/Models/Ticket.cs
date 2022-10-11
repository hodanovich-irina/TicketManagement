using System;

namespace TicketManagement.DataAccess.Models
{
    /// <summary>
    /// Class for describe ticket.
    /// </summary>
    public class Ticket
    {
        public int Id { get; set; }

        public decimal Price { get; set; }

        public string UserId { get; set; }

        public int EventSeatId { get; set; }

        public DateTime DateOfPurchase { get; set; }
    }
}
