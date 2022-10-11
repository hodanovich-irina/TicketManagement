using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketManagement.TicketAPI.Dto
{
    public class TicketInfo
    {
        public int Id { get; set; }

        public decimal Price { get; set; }

        public int EventSeatId { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        public string EventName { get; set; }

        public int Row { get; set; }

        public int Number { get; set; }

        public DateTime DateOfPurchase { get; set; }
    }
}
