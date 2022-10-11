using System.Collections.Generic;
using TicketManagement.Presentation.Dto;

namespace TicketManagement.Presentation.Models
{
    /// <summary>
    /// Class for describe area.
    /// </summary>
    public class AreaViewModel
    {
        public int Id { get; set; }

        public int LayoutId { get; set; }

        public string LayoutName { get; set; }

        public List<LayoutDto> Layouts { get; set; }

        public string Description { get; set; }

        public int CoordX { get; set; }

        public int CoordY { get; set; }
    }
}
