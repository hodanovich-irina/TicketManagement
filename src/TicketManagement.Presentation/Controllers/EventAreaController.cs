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
    /// Controller for work with event areas.
    /// </summary>
    [Authorize(Roles = Role.User + ", " + Role.Admin + ", " + Role.EventManager)]
    public class EventAreaController : Controller
    {
        private readonly IEventRestClient _eventRestClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAreaController"/> class.
        /// Class constructor.
        /// </summary>
        /// <param name="eventRestClient">event rest client.</param>
        public EventAreaController(IEventRestClient eventRestClient)
        {
            _eventRestClient = eventRestClient;
        }

        /// <summary>
        /// Method for select event area data.
        /// </summary>
        /// <param name="id">event id.</param>
        /// <returns>view result.</returns>
        public async Task<IActionResult> Index(int id)
        {
            var eventAreas = await _eventRestClient.GetEventAreaByParentIdAsync(id, HttpContext.Request.Cookies["secret_jwt_key"]);
            var eventToEventArea = await _eventRestClient.GetEventByIdAsync(id, HttpContext.Request.Cookies["secret_jwt_key"]);
            var eventAreasWithEvent = new List<EventAreaViewModel>();
            foreach (var eventArea in eventAreas)
            {
                eventAreasWithEvent.Add(new EventAreaViewModel
                {
                    Id = eventArea.Id,
                    CoordX = eventArea.CoordX,
                    CoordY = eventArea.CoordY,
                    Description = eventArea.Description,
                    EventId = eventArea.EventId,
                    EventName = eventToEventArea.Name,
                    Price = eventArea.Price,
                });
            }

            return View(eventAreasWithEvent);
        }
    }
}
