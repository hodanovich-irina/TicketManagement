using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Presentation.Client;
using TicketManagement.Presentation.Models;
using TicketManagement.Presentation.RoleData;

namespace TicketManagement.Presentation.Controllers
{
    /// <summary>
    /// Controller for work with event seats.
    /// </summary>
    [Authorize(Roles = Role.User + ", " + Role.Admin + ", " + Role.EventManager)]
    public class EventSeatController : Controller
    {
        private readonly IEventRestClient _eventRestClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventSeatController"/> class.
        /// Class constructor.
        /// </summary>
        /// <param name="eventRestClient">event rest client.</param>
        public EventSeatController(IEventRestClient eventRestClient)
        {
            _eventRestClient = eventRestClient;
        }

        /// <summary>
        /// Method for select event seat data.
        /// </summary>
        /// <param name="id">event area id.</param>
        /// <returns>view result.</returns>
        public async Task<IActionResult> Index(int id)
        {
            var seats = await _eventRestClient.GetEventSeatByParentIdAsync(id, HttpContext.Request.Cookies["secret_jwt_key"]);
            var area = await _eventRestClient.GetEventAreaByIdAsync(id, HttpContext.Request.Cookies["secret_jwt_key"]);
            var eventToIndex = await _eventRestClient.GetEventByIdAsync(area.EventId, HttpContext.Request.Cookies["secret_jwt_key"]);
            var eventSeatsWithArea = new List<EventSeatViewModel>();
            foreach (var seat in seats)
            {
                eventSeatsWithArea.Add(new EventSeatViewModel
                {
                    Id = seat.Id,
                    Number = seat.Number,
                    Row = seat.Row,
                    AreaY = area.CoordY,
                    AreaX = area.CoordX,
                    EventAreaId = area.Id,
                    State = seat.State,
                    EventName = eventToIndex.Name,
                });
            }

            return View(eventSeatsWithArea);
        }
    }
}
