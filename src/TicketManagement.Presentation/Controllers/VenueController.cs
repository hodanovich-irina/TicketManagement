using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Presentation.Client;
using TicketManagement.Presentation.Dto;
using TicketManagement.Presentation.Filters;
using TicketManagement.Presentation.RoleData;

namespace TicketManagement.Presentation.Controllers
{
    /// <summary>
    /// Controller for work with venues.
    /// </summary>
    [Authorize(Roles = Role.Admin + ", " + Role.VenueManager)]

    public class VenueController : Controller
    {
        private readonly IVenueRestClient _venueClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="VenueController"/> class.
        /// Class constructor.
        /// </summary>
        /// <param name="venueClient">venue service.</param>
        public VenueController(IVenueRestClient venueClient)
        {
            _venueClient = venueClient;
        }

        /// <summary>
        /// Method for select venue data.
        /// </summary>
        /// <returns>view result.</returns>
        public async Task<IActionResult> Index()
        {
            var venues = await _venueClient.GetAllVenueAsync(HttpContext.Request.Cookies["secret_jwt_key"]);

            return View(venues);
        }

        /// <summary>
        /// Method for create venue data.
        /// </summary>
        /// <returns>view result.</returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Method for create venue data.
        /// </summary>
        /// <param name="venue">Object of venue.</param>
        /// <returns>redirect to action.</returns>
        [HttpPost]
        [ValidationExceptionFilter]
        public async Task<IActionResult> Create(VenueDto venue)
        {
            await _venueClient.AddVenueAsync(venue, HttpContext.Request.Cookies["secret_jwt_key"]);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Method for edit venue data.
        /// </summary>
        /// <param name="id">venue id.</param>
        /// <returns>view result.</returns>
        public async Task<IActionResult> Edit(int id)
        {
            var venue = await _venueClient.GetVenueByIdAsync(id, HttpContext.Request.Cookies["secret_jwt_key"]);
            return View(venue);
        }

        /// <summary>
        /// Method for edit venue data.
        /// </summary>
        /// <param name="venue">Object of venue.</param>
        /// <returns>redirect to action.</returns>
        [HttpPost]
        [ValidationExceptionFilter]
        public async Task<IActionResult> Edit(VenueDto venue)
        {
            await _venueClient.EditVenueAsync(venue, HttpContext.Request.Cookies["secret_jwt_key"]);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Method for delete venue data.
        /// </summary>
        /// <param name="id">venue id.</param>
        /// <returns>view result.</returns>
        [HttpGet]
        [ActionName("Delete")]
#pragma warning disable S4144 // Methods should not have identical implementations
        public async Task<IActionResult> ConfirmDelete(int id)
#pragma warning restore S4144 // Methods should not have identical implementations
        {
            var venue = await _venueClient.GetVenueByIdAsync(id, HttpContext.Request.Cookies["secret_jwt_key"]);
            return View(venue);
        }

        /// <summary>
        /// Method for delete venue data.
        /// </summary>
        /// <param name="id">venue id.</param>
        /// <returns>redirect to action.</returns>
        [HttpPost]
        [ValidationExceptionFilter]
        public async Task<IActionResult> Delete(int id)
        {
            await _venueClient.DeleteVenueAsync(id, HttpContext.Request.Cookies["secret_jwt_key"]);
            return RedirectToAction("Index");
        }
    }
}