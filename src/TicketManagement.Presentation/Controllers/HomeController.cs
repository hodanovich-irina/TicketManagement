using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using TicketManagement.Presentation.Client;
using TicketManagement.Presentation.Dto;
using TicketManagement.Presentation.Filters;
using TicketManagement.Presentation.Models;
using TicketManagement.Presentation.RoleData;
using TicketManagement.Presentation.Settings;

namespace TicketManagement.Presentation.Controllers
{
    /// <summary>
    /// Controller for work with events.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IVenueRestClient _venueRestClient;
        private readonly IEventRestClient _eventRestClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// Class constructor.
        /// </summary>
        /// <param name="eventRestClient">event rest client.</param>
        /// <param name="venueRestClient">venue rest client.</param>
        public HomeController(IEventRestClient eventRestClient, IVenueRestClient venueRestClient)
        {
            _eventRestClient = eventRestClient;
            _venueRestClient = venueRestClient;
        }

        /// <summary>
        /// Method for select event data.
        /// </summary>
        /// <returns>view result.</returns>
        [FeatureGate(FeatureFlags.PresentationUI)]
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Request.Cookies["secret_jwt_key"];
            var events = await _eventRestClient.GetAllEventAsync(token);
            var eventsModel = new List<EventViewModel>();
            foreach (var eventModel in events)
            {
                var layout = await _venueRestClient.GetLayoutByIdAsync(eventModel.LayoutId, HttpContext.Request.Cookies["secret_jwt_key"]);
                var venue = await _venueRestClient.GetVenueByIdAsync(layout.VenueId, HttpContext.Request.Cookies["secret_jwt_key"]);
                eventsModel.Add(new EventViewModel
                {
                    Id = eventModel.Id,
                    DateEnd = eventModel.DateEnd,
                    DateStart = eventModel.DateStart,
                    Description = eventModel.Description,
                    ImageURL = eventModel.ImageURL,
                    LayoutName = layout.Name,
                    ShowTime = new TimeSpan(eventModel.Hours, eventModel.Minutes, eventModel.Seconds),
                    Name = eventModel.Name,
                    VenueName = venue.Name,
                    BaseAreaPrice = eventModel.BaseAreaPrice,
                });
            }

            ViewData["TimeZone"] = HttpContext.Request.Cookies["timeZone"];

            return View(eventsModel);
        }

        /// <summary>
        /// Method for create event data.
        /// </summary>
        /// <returns>view result.</returns>
        [Authorize(Roles = Role.Admin + ", " + Role.EventManager)]
        [FeatureGate(FeatureFlags.PresentationUI)]
        public async Task<IActionResult> Create()
        {
            var layouts = await _venueRestClient.GetAllLayoutAsync(HttpContext.Request.Cookies["secret_jwt_key"]);
            layouts.Select(layout => new LayoutDto
            {
                Id = layout.Id,
                Name = layout.Name,
            });

            return View(new EventCreateViewModel { Layouts = layouts.ToList() });
        }

        /// <summary>
        /// Method for create event data.
        /// </summary>
        /// <param name="eventToCreate">Object of event.</param>
        /// <returns>redirect to action.</returns>
        [HttpPost]
        [ValidationExceptionFilter]
        [FeatureGate(FeatureFlags.PresentationUI)]
        public async Task<IActionResult> Create(EventCreateViewModel eventToCreate)
        {
            var model = new EventDto
            {
                Id = eventToCreate.Id,
                DateEnd = eventToCreate.DateEnd,
                DateStart = eventToCreate.DateStart,
                Description = eventToCreate.Description,
                ImageURL = eventToCreate.ImageURL,
                ShowTime = eventToCreate.ShowTime,
                Name = eventToCreate.Name,
                LayoutId = eventToCreate.LayoutId,
                BaseAreaPrice = eventToCreate.BaseAreaPrice,
            };

            await _eventRestClient.AddEventAsync(ReturnModel(model), HttpContext.Request.Cookies["secret_jwt_key"]);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Method for edit event data.
        /// </summary>
        /// <param name="id">event id.</param>
        /// <returns>view result.</returns>
        [Authorize(Roles = Role.Admin + ", " + Role.EventManager)]
        [FeatureGate(FeatureFlags.PresentationUI)]
        public async Task<IActionResult> Edit(int id)
        {
            var layouts = await _venueRestClient.GetAllLayoutAsync(HttpContext.Request.Cookies["secret_jwt_key"]);
            layouts.Select(layout => new LayoutDto
            {
                Id = layout.Id,
                Name = layout.Name,
            });

            var eventToEdit = await _eventRestClient.GetEventByIdAsync(id, HttpContext.Request.Cookies["secret_jwt_key"]);

            var model = new EventCreateViewModel
            {
                Id = eventToEdit.Id,
                DateEnd = eventToEdit.DateEnd,
                DateStart = eventToEdit.DateStart,
                Description = eventToEdit.Description,
                ImageURL = eventToEdit.ImageURL,
                ShowTime = new TimeSpan(eventToEdit.Hours, eventToEdit.Minutes, eventToEdit.Seconds),
                Name = eventToEdit.Name,
                LayoutId = eventToEdit.LayoutId,
                BaseAreaPrice = eventToEdit.BaseAreaPrice,
                Layouts = layouts.ToList(),
            };
            return View(model);
        }

        /// <summary>
        /// Method for edit event data.
        /// </summary>
        /// <param name="eventDto">Object of event.</param>
        /// <returns>redirect to action.</returns>
        [HttpPost]
        [ValidationExceptionFilter]
        [FeatureGate(FeatureFlags.PresentationUI)]
        public async Task<IActionResult> Edit(EventDto eventDto)
        {
            await _eventRestClient.EditEventAsync(ReturnModel(eventDto), HttpContext.Request.Cookies["secret_jwt_key"]);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Method for delete event data.
        /// </summary>
        /// <param name="id">event id.</param>
        /// <returns>view result.</returns>
        [Authorize(Roles = Role.Admin + ", " + Role.EventManager)]
        [HttpGet]
        [ActionName("Delete")]
        [FeatureGate(FeatureFlags.PresentationUI)]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var eventDto = await _eventRestClient.GetEventByIdAsync(id, HttpContext.Request.Cookies["secret_jwt_key"]);
            var layout = await _venueRestClient.GetLayoutByIdAsync(eventDto.LayoutId, HttpContext.Request.Cookies["secret_jwt_key"]);
            var eventToDelete = new EventCreateViewModel
            {
                Id = id,
                Description = eventDto.Description,
                Name = eventDto.Name,
                DateStart = eventDto.DateStart,
                DateEnd = eventDto.DateEnd,
                ImageURL = eventDto.ImageURL,
                LayoutId = eventDto.LayoutId,
                ShowTime = new TimeSpan(eventDto.Hours, eventDto.Minutes, eventDto.Seconds),
                LayoutName = layout.Name,
            };

            return View(eventToDelete);
        }

        /// <summary>
        /// Method for delete event data.
        /// </summary>
        /// <param name="id">event id.</param>
        /// <returns>redirect to action.</returns>
        [HttpPost]
        [ValidationExceptionFilter]
        [FeatureGate(FeatureFlags.PresentationUI)]
        public async Task<IActionResult> Delete(int id)
        {
            await _eventRestClient.DeleteEventAsync(id, HttpContext.Request.Cookies["secret_jwt_key"]);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Method for set cokies for langeage.
        /// </summary>
        /// <param name="culture">culture name.</param>
        /// <param name="returnUrl">Url.</param>
        /// <returns>local redirect.</returns>
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            return LocalRedirect(returnUrl);
        }

        /// <summary>
        /// Method for get error message.
        /// </summary>
        /// <returns>view result.</returns>
        [FeatureGate(FeatureFlags.PresentationUI)]
        public ViewResult ErrorMessage()
        {
            ViewData["Message"] = HttpContext.Request.Cookies["message"];
            return View();
        }

        private EventModel ReturnModel(EventDto eventDto)
        {
            var eventModel = new EventModel
            {
                BaseAreaPrice = eventDto.BaseAreaPrice,
                DateEnd = eventDto.DateEnd,
                DateStart = eventDto.DateStart,
                Description = eventDto.Description,
                Hours = eventDto.ShowTime.Hours,
                Id = eventDto.Id,
                ImageURL = eventDto.ImageURL,
                LayoutId = eventDto.LayoutId,
                Minutes = eventDto.ShowTime.Minutes,
                Name = eventDto.Name,
                Seconds = eventDto.ShowTime.Seconds,
            };

            return eventModel;
        }
    }
}
