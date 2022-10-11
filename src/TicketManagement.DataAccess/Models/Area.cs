namespace TicketManagement.DataAccess.Models
{
    /// <summary>
    /// Class for describe area.
    /// </summary>
    public class Area
    {
        public int Id { get; set; }

        public int LayoutId { get; set; }

        public string Description { get; set; }

        public int CoordX { get; set; }

        public int CoordY { get; set; }
    }
}
