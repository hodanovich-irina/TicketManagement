using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using RestEase;
using TicketManagement.Presentation.Dto;
using TicketManagement.Presentation.Models;

namespace TicketManagement.Presentation.Client
{
    /// <summary>
    /// Interface for event rest client.
    /// </summary>
    public interface IEventRestClient
    {
        /// <summary>
        /// Method for get all events.
        /// </summary>
        /// <param name="token">token.</param>
        /// <returns>events.</returns>
        [Get("Event/GetAll")]
        Task<IEnumerable<EventModel>> GetAllEventAsync([Header("Authorization")] string token);

        /// <summary>
        /// Method for get all event areas.
        /// </summary>
        /// <param name="token">token.</param>
        /// <returns>event areas.</returns>
        [Get("EventArea/GetAll")]
        Task<IEnumerable<EventAreaDto>> GetAllEventAreaAsync([Header("Authorization")] string token);

        /// <summary>
        /// Method for get all event seats.
        /// </summary>
        /// <param name="token">token.</param>
        /// <returns>event seats.</returns>
        [Get("EventSeat/GetAll")]
        Task<IEnumerable<EventSeatDto>> GetAllEventSeatAsync([Header("Authorization")] string token);

        /// <summary>
        /// Method for get event by id.
        /// </summary>
        /// <param name="id">event id.</param>
        /// <param name="token">token.</param>
        /// <returns>event.</returns>
        [Get("Event/GetById")]
        Task<EventModel> GetEventByIdAsync(int id, [Header("Authorization")] string token);

        /// <summary>
        /// Method for get event area by id.
        /// </summary>
        /// <param name="id">event area id.</param>
        /// <param name="token">token.</param>
        /// <returns>event area.</returns>
        [Get("EventArea/GetById")]
        Task<EventAreaDto> GetEventAreaByIdAsync(int id, [Header("Authorization")] string token);

        /// <summary>
        /// Method for get event seat by id.
        /// </summary>
        /// <param name="id">event seat id.</param>
        /// <param name="token">token.</param>
        /// <returns>event seat.</returns>
        [Get("EventSeat/GetById")]
        Task<EventSeatDto> GetEventSeatByIdAsync(int id, [Header("Authorization")] string token);

        /// <summary>
        /// Method for get events by parent id.
        /// </summary>
        /// <param name="id">parent id.</param>
        /// <param name="token">token.</param>
        /// <returns>events.</returns>
        [Get("Event/GetByParenId")]
        Task<IEnumerable<EventDto>> GetEventByParentIdAsync(int id, [Header("Authorization")] string token);

        /// <summary>
        /// Method for get event areas by parent id.
        /// </summary>
        /// <param name="id">parent id.</param>
        /// <param name="token">token.</param>
        /// <returns>event areas.</returns>
        [Get("EventArea/GetByParenId")]
        Task<IEnumerable<EventAreaDto>> GetEventAreaByParentIdAsync(int id, [Header("Authorization")] string token);

        /// <summary>
        /// Method for get event seats by parent id.
        /// </summary>
        /// <param name="id">parent id.</param>
        /// <param name="token">token.</param>
        /// <returns>seats.</returns>
        [Get("EventSeat/GetByParenId")]
        Task<IEnumerable<EventSeatDto>> GetEventSeatByParentIdAsync(int id, [Header("Authorization")] string token);

        /// <summary>
        /// Method for delete event data.
        /// </summary>
        /// <param name="id">event id.</param>
        /// <param name="token">token.</param>
        /// <returns>redirect action.</returns>
        [Delete("Event/Delete")]
        Task<bool> DeleteEventAsync(int id, [Header("Authorization")] string token);

        /// <summary>
        /// Method for delete event area data.
        /// </summary>
        /// <param name="id">event area id.</param>
        /// <param name="token">token.</param>
        /// <returns>redirect action.</returns>
        [Delete("EventArea/Delete")]
        Task<bool> DeleteEventAreaAsync(int id, [Header("Authorization")] string token);

        /// <summary>
        /// Method for delete event seat data.
        /// </summary>
        /// <param name="id">event seat id.</param>
        /// <param name="token">token.</param>
        /// <returns>redirect action.</returns>
        [Delete("EventSeat/Delete")]
        Task<bool> DeleteEventSeatAsync(int id, [Header("Authorization")] string token);

        /// <summary>
        /// Method for create event.
        /// </summary>
        /// <param name="model">Object of event.</param>
        /// <param name="token">token.</param>
        /// <returns>task.</returns>
        [Post("Event/Add")]
        Task AddEventAsync([Body] EventModel model, [Header("Authorization")] string token);

        /// <summary>
        /// Method for create event area.
        /// </summary>
        /// <param name="model">Object of event area.</param>
        /// <param name="token">token.</param>
        /// <return>event area.</return>
        [Post("EventArea/Add")]
        Task<EventAreaDto> AddEventAreaAsync([Body] EventAreaDto model, [Header("Authorization")] string token);

        /// <summary>
        /// Method for create event seat.
        /// </summary>
        /// <param name="model">Object of event seat.</param>
        /// <return>event seat.</return>
        [Post("EventSeat/Add")]
        Task<EventSeatDto> AddEventSeatAsync([Body] EventSeatDto model);

        /// <summary>
        /// Method for edit event data.
        /// </summary>
        /// <param name="model">event model.</param>
        /// <param name="token">token.</param>
        /// <returns>truthfulness of update.</returns>
        [Put("Event/Update")]
        Task<bool> EditEventAsync([Body] EventModel model, [Header("Authorization")] string token);

        /// <summary>
        /// Method for edit event area data.
        /// </summary>
        /// <param name="model">event area model.</param>
        /// <param name="token">token.</param>
        /// <returns>truthfulness of update.</returns>
        [Put("EventArea/Update")]
        Task<bool> EditEventAreaAsync([Body] EventAreaDto model, [Header("Authorization")] string token);

        /// <summary>
        /// Method for edit event seat data.
        /// </summary>
        /// <param name="model">event seat model.</param>
        /// <param name="token">token.</param>
        /// <returns>truthfulness of update.</returns>
        [Put("EventSeat/Update")]
        Task<bool> EditEventSeatAsync([Body] EventSeatDto model, [Header("Authorization")] string token);

        /// <summary>
        /// Method for get all venues.
        /// </summary>
        /// <param name="token">token.</param>
        /// <returns>venues.</returns>
        [Get("Venue/GetAll")]
        Task<IEnumerable<VenueDto>> GetAllVenueAsync([Header("Authorization")] string token);

        /// <summary>
        /// Method for get all layouts.
        /// </summary>
        /// <param name="token">token.</param>
        /// <returns>layouts.</returns>
        [Get("Layout/GetAll")]
        Task<IEnumerable<LayoutDto>> GetAllLayoutAsync([Header("Authorization")] string token);

        /// <summary>
        ///  Method for upload file.
        /// </summary>
        /// <param name="uploadedFile">uploaded file.</param>
        /// <param name="token">token.</param>
        /// <returns>task.</returns>
        [Post("ThirdPartyImport/UploadFile")]
        Task UploadFile([Body] MultipartFormDataContent uploadedFile, [Header("Authorization")] string token);
    }
}