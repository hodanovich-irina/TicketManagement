using System.Collections.Generic;
using TicketManagement.Presentation.Dto;

namespace TicketManagement.Presentation.Models
{
    /// <summary>
    /// Class for describe seat.
    /// </summary>
    public class SeatViewModel
    {
        public int Id { get; set; }

        public int AreaId { get; set; }

        public int AreaX { get; set; }

        public int AreaY { get; set; }

        public string LayoutName { get; set; }

        public List<AreaDto> Areas { get; set; }

        public int Row { get; set; }

        public int Number { get; set; }
    }
}
