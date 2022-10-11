using System.Collections.Generic;
using TicketManagement.EventAPI.Dto;

namespace TicketManagement.EventAPI.ImportThirdPartyEvent
{
    /// <summary>
    /// Interface for third party event repository.
    /// </summary>
    public interface IThirdPartyEventRepository
    {
        /// <summary>
        /// Method for read json file with third party event.
        /// </summary>
        /// <param name="path">path of file.</param>
        /// <returns>collection of third party event.</returns>
        IEnumerable<ThirdPartyEventViewModel> Read(string path);
    }
}
