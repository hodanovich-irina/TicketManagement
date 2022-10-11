﻿namespace TicketManagement.Presentation.Dto
{
    /// <summary>
    /// Class DTO for describe seat.
    /// </summary>
    public class SeatDto
    {
        public int Id { get; set; }

        public int AreaId { get; set; }

        public int Row { get; set; }

        public int Number { get; set; }
    }
}
