using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TicketManagement.EventAPI.ImportThirdPartyEvent
{
    /// <summary>
    /// Interface dor third party event service.
    /// </summary>
    public interface IThirdPartyEventService
    {
        /// <summary>
        /// Method for add event.
        /// </summary>
        /// <param name="uploadedFile">upload third party events file.</param>
        Task AddEvent(IFormFile uploadedFile);
    }
}
