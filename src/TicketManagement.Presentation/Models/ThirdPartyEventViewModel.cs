using System;

namespace TicketManagement.Presentation.Models
{
    /// <summary>
    /// Class for describe third party event.
    /// </summary>
    public class ThirdPartyEventViewModel
    {
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Description { get; set; }

        public string PosterImage { get; set; }

        public int Id { get; set; }

        public bool TrueEvent { get; set; }

        public string VenueName { get; set; }

        public string LayoutName { get; set; }
    }
}
