using System.Collections.Generic;
using TicketManagement.Presentation.Dto;

namespace TicketManagement.Presentation.Models
{
    /// <summary>
    /// Class for describe layout.
    /// </summary>
    public class LayoutCreateViewModel
    {
        public int Id { get; set; }

        public int VenueId { get; set; }

        public string VenueName { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public List<VenueDto> Venues { get; set; }
    }
}
