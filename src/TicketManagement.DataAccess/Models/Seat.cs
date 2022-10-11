namespace TicketManagement.DataAccess.Models
{
    /// <summary>
    /// Class for describe seat.
    /// </summary>
    public class Seat
    {
        public int Id { get; set; }

        public int AreaId { get; set; }

        public int Row { get; set; }

        public int Number { get; set; }
    }
}
