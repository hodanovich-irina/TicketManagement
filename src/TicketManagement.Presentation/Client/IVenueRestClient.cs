using System.Collections.Generic;
using System.Threading.Tasks;
using RestEase;
using TicketManagement.Presentation.Dto;

namespace TicketManagement.Presentation.Client
{
    /// <summary>
    /// Interface for venue rest client.
    /// </summary>
    public interface IVenueRestClient
    {
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
        /// Method for get all areas.
        /// </summary>
        /// <param name="token">token.</param>
        /// <returns>areas.</returns>
        [Get("Area/GetAll")]
        Task<IEnumerable<AreaDto>> GetAllAreaAsync([Header("Authorization")] string token);

        /// <summary>
        /// Method for get all seats.
        /// </summary>
        /// <param name="token">token.</param>
        /// <returns>seats.</returns>
        [Get("Seat/GetAll")]
        Task<IEnumerable<SeatDto>> GetAllSeatAsync([Header("Authorization")] string token);

        /// <summary>
        /// Method for get venue by id.
        /// </summary>
        /// <param name="id">venue id.</param>
        /// <param name="token">token.</param>
        /// <returns>venue.</returns>
        [Get("Venue/GetById")]
        Task<VenueDto> GetVenueByIdAsync(int id, [Header("Authorization")] string token);

        /// <summary>
        /// Method for get layout by id.
        /// </summary>
        /// <param name="id">layout id.</param>
        /// <param name="token">token.</param>
        /// <returns>layout.</returns>
        [Get("Layout/GetById")]
        Task<LayoutDto> GetLayoutByIdAsync(int id, [Header("Authorization")] string token);

        /// <summary>
        /// Method for get area by id.
        /// </summary>
        /// <param name="id">area id.</param>
        /// <param name="token">token.</param>
        /// <returns>area.</returns>
        [Get("Area/GetById")]
        Task<AreaDto> GetAreaByIdAsync(int id, [Header("Authorization")] string token);

        /// <summary>
        /// Method for get seat by id.
        /// </summary>
        /// <param name="id">seat id.</param>
        /// <param name="token">token.</param>
        /// <returns>seat.</returns>
        [Get("Seat/GetById")]
        Task<SeatDto> GetSeatByIdAsync(int id, [Header("Authorization")] string token);

        /// <summary>
        /// Method for get layout by parent id.
        /// </summary>
        /// <param name="id">parent id.</param>
        /// <param name="token">token.</param>
        /// <returns>layout.</returns>
        [Get("Layout/GetByParenId")]
        Task<IEnumerable<LayoutDto>> GetLayoutByParentIdAsync(int id, [Header("Authorization")] string token);

        /// <summary>
        /// Method for get area by parent id.
        /// </summary>
        /// <param name="id">parent id.</param>
        /// <param name="token">token.</param>
        /// <returns>area.</returns>
        [Get("Area/GetByParenId")]
        Task<IEnumerable<AreaDto>> GetAreaByParentIdAsync(int id, [Header("Authorization")] string token);

        /// <summary>
        /// Method for get seat by parent id.
        /// </summary>
        /// <param name="id">parent id.</param>
        /// <param name="token">token.</param>
        /// <returns>seat.</returns>
        [Get("Seat/GetByParenId")]
        Task<IEnumerable<SeatDto>> GetSeatByParentIdAsync(int id, [Header("Authorization")] string token);

        /// <summary>
        /// Method for delete venue data.
        /// </summary>
        /// <param name="id">venue id.</param>
        /// <param name="token">token.</param>
        /// <returns>redirect action.</returns>
        [Delete("Venue/Delete")]
        Task<bool> DeleteVenueAsync(int id, [Header("Authorization")] string token);

        /// <summary>
        /// Method for delete layout data.
        /// </summary>
        /// <param name="id">layout id.</param>
        /// <param name="token">token.</param>
        /// <returns>redirect action.</returns>
        [Delete("Layout/Delete")]
        Task<bool> DeleteLayoutAsync(int id, [Header("Authorization")] string token);

        /// <summary>
        /// Method for delete area data.
        /// </summary>
        /// <param name="id">area id.</param>
        /// <param name="token">token.</param>
        /// <returns>redirect action.</returns>
        [Delete("Area/Delete")]
        Task<bool> DeleteAreaAsync(int id, [Header("Authorization")] string token);

        /// <summary>
        /// Method for delete seat data.
        /// </summary>
        /// <param name="id">seat id.</param>
        /// <param name="token">token.</param>
        /// <returns>redirect action.</returns>
        [Delete("Seat/Delete")]
        Task<bool> DeleteSeatAsync(int id, [Header("Authorization")] string token);

        /// <summary>
        /// Method for create venue.
        /// </summary>
        /// <param name="model">Object of venue.</param>
        /// <param name="token">token.</param>
        /// <returns>task.</returns>
        [Post("Venue/Add")]
        Task AddVenueAsync([Body] VenueDto model, [Header("Authorization")] string token);

        /// <summary>
        /// Method for create layout.
        /// </summary>
        /// <param name="model">Object of layout.</param>
        /// <param name="token">token.</param>
        /// <returns>task.</returns>
        [Post("Layout/Add")]
        Task AddLayoutAsync([Body] LayoutDto model, [Header("Authorization")] string token);

        /// <summary>
        /// Method for create area.
        /// </summary>
        /// <param name="model">Object of area.</param>
        /// <param name="token">token.</param>
        /// <returns>task.</returns>
        [Post("Area/Add")]
        Task AddAreaAsync([Body] AreaDto model, [Header("Authorization")] string token);

        /// <summary>
        /// Method for create seat.
        /// </summary>
        /// <param name="model">Object of seat.</param>
        /// <param name="token">token.</param>
        /// <returns>task.</returns>
        [Post("Seat/Add")]
        Task AddSeatAsync([Body] SeatDto model, [Header("Authorization")] string token);

        /// <summary>
        /// Method for edit venue data.
        /// </summary>
        /// <param name="model">venue model.</param>
        /// <param name="token">token.</param>
        /// <returns>truthfulness of update.</returns>
        [Put("Venue/Update")]
        Task<bool> EditVenueAsync([Body] VenueDto model, [Header("Authorization")] string token);

        /// <summary>
        /// Method for edit layout data.
        /// </summary>
        /// <param name="model">layout model.</param>
        /// <param name="token">token.</param>
        /// <returns>truthfulness of update.</returns>
        [Put("Layout/Update")]
        Task<bool> EditLayoutAsync([Body] LayoutDto model, [Header("Authorization")] string token);

        /// <summary>
        /// Method for edit area data.
        /// </summary>
        /// <param name="model">area model.</param>
        /// <param name="token">token.</param>
        /// <returns>truthfulness of update.</returns>
        [Put("Area/Update")]
        Task<bool> EditAreaAsync([Body] AreaDto model, [Header("Authorization")] string token);

        /// <summary>
        /// Method for edit seat data.
        /// </summary>
        /// <param name="model">seat model.</param>
        /// <param name="token">token.</param>
        /// <returns>truthfulness of update.</returns>
        [Put("Seat/Update")]
        Task<bool> EditSeatAsync([Body] SeatDto model, [Header("Authorization")] string token);
    }
}