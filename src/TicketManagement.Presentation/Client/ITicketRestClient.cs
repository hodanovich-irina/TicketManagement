using System.Collections.Generic;
using System.Threading.Tasks;
using RestEase;
using TicketManagement.Presentation.Dto;

namespace TicketManagement.Presentation.Client
{
    /// <summary>
    /// Interface for ticket rest client.
    /// </summary>
    public interface ITicketRestClient
    {
        /// <summary>
        /// Method for get all areas.
        /// </summary>
        /// <param name="token">token.</param>
        /// <returns>tickets.</returns>
        [Get("Ticket/GetAll")]
        Task<IEnumerable<TicketDto>> GetAllTicketAsync([Header("Authorization")] string token);

        /// <summary>
        /// Method for get ticket by id.
        /// </summary>
        /// <param name="id">ticket id.</param>
        /// <param name="token">token.</param>
        /// <returns>ticket.</returns>
        [Get("Ticket/GetById")]
        Task<TicketDto> GetTicketByIdAsync(int id, [Header("Authorization")] string token);

        /// <summary>
        /// Method for get ticket by parent id.
        /// </summary>
        /// <param name="id">parent id.</param>
        /// <param name="token">token.</param>
        /// <returns>ticket.</returns>
        [Get("Ticket/GetByParenId")]
        Task<IEnumerable<TicketDto>> GetTicketByParentIdAsync(int id, [Header("Authorization")] string token);

        /// <summary>
        /// Method for get ticket by parent string id.
        /// </summary>
        /// <param name="id">parent string id.</param>
        /// <param name="token">token.</param>
        /// <returns>ticket.</returns>
        [Get("Ticket/GetByParentStringId")]
        Task<IEnumerable<TicketDto>> GetTicketByParentStringIdAsync(string id, [Header("Authorization")] string token);

        /// <summary>
        /// Method for delete ticket data.
        /// </summary>
        /// <param name="id">layout id.</param>
        /// <param name="token">token.</param>
        /// <returns>truthfulness of delete.</returns>
        [Delete("Ticket/Delete")]
        Task<bool> DeleteTicketAsync(int id, [Header("Authorization")] string token);

        /// <summary>
        /// Method for create ticket.
        /// </summary>
        /// <param name="model">Object of ticket.</param>
        /// <param name="token">token.</param>
        /// <return>ticket.</return>
        [Post("Ticket/Add")]
        Task AddTicketAsync([Body] TicketDto model, [Header("Authorization")] string token);

        /// <summary>
        /// Method for edit ticket data.
        /// </summary>
        /// <param name="model">ticket model.</param>
        /// <param name="token">token.</param>
        /// <returns>truthfulness of update.</returns>
        [Put("Ticket/Update")]
        Task<bool> EditTicketAsync([Body] TicketDto model, [Header("Authorization")] string token);
    }
}
