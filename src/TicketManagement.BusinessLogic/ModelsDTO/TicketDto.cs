using System;

namespace TicketManagement.BusinessLogic.ModelsDTO
{
    /// <summary>
    /// Class DTO for describe ticket.
    /// </summary>
    public class TicketDto
    {
        public int Id { get; set; }

        public decimal Price { get; set; }

        public string UserId { get; set; }

        public int EventSeatId { get; set; }

        public DateTime DateOfPurchase { get; set; }
    }
}
