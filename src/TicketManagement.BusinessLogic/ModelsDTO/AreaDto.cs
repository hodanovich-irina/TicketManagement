namespace TicketManagement.BusinessLogic.ModelsDTO
{
    /// <summary>
    /// Class DTO for describe area.
    /// </summary>
    public class AreaDto
    {
        public int Id { get; set; }

        public int LayoutId { get; set; }

        public string Description { get; set; }

        public int CoordX { get; set; }

        public int CoordY { get; set; }
    }
}
