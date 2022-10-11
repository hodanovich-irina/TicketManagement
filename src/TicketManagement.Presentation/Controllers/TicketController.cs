using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Presentation.Client;
using TicketManagement.Presentation.Dto;
using TicketManagement.Presentation.Filters;
using TicketManagement.Presentation.Models;
using TicketManagement.Presentation.RoleData;

namespace TicketManagement.Presentation.Controllers
{
    /// <summary>
    /// Controller for work with tickets.
    /// </summary>
    public class TicketController : Controller
    {
        private readonly ITicketRestClient _ticketClient;
        private readonly IEventRestClient _eventClient;
        private readonly IUserRestClient _userClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketController"/> class.
        /// Class constructor.
        /// </summary>
        /// <param name="ticketClient">ticket client.</param>
        /// <param name="eventClient">event client.</param>
        /// <param name="userClient">user client.</param>
        public TicketController(ITicketRestClient ticketClient, IEventRestClient eventClient, IUserRestClient userClient)
        {
            _ticketClient = ticketClient;
            _eventClient = eventClient;
            _userClient = userClient;
        }

        /// <summary>
        /// Method for select ticket data.
        /// </summary>
        /// <returns>view result.</returns>
        [Authorize(Roles = Role.User + ", " + Role.Admin + ", " + Role.EventManager)]
        public async Task<IActionResult> Index()
        {
            var tickets = await _ticketClient.GetAllTicketAsync(HttpContext.Request.Cookies["secret_jwt_key"]);
            var ticketsModel = new List<TicketViewModel>();
            foreach (var ticketModel in tickets)
            {
                var seat = await _eventClient.GetEventSeatByIdAsync(ticketModel.EventSeatId, HttpContext.Request.Cookies["secret_jwt_key"]);
                var area = await _eventClient.GetEventAreaByIdAsync(seat.EventAreaId, HttpContext.Request.Cookies["secret_jwt_key"]);
                var eventInArea = await _eventClient.GetEventByIdAsync(area.EventId, HttpContext.Request.Cookies["secret_jwt_key"]);
                var user = await _userClient.GetUserById(ticketModel.UserId);
                ticketsModel.Add(new TicketViewModel
                {
                    Id = ticketModel.Id,
                    Price = ticketModel.Price,
                    DateOfPurchase = ticketModel.DateOfPurchase,
                    Number = seat.Number,
                    Row = seat.Row,
                    UserName = user.UserName,
                    EventDateEnd = eventInArea.DateEnd,
                    EventDateStart = eventInArea.DateStart,
                    EventName = eventInArea.Name,
                });
            }

            return View(ticketsModel);
        }

        /// <summary>
        /// Method for create ticket data.
        /// </summary>
        /// <returns>view result.</returns>
        [Authorize(Roles = Role.User)]
        public async Task<IActionResult> Create(int id)
        {
            var eventSeat = await _eventClient.GetEventSeatByIdAsync(id, HttpContext.Request.Cookies["secret_jwt_key"]);
            var eventArea = await _eventClient.GetEventAreaByIdAsync(eventSeat.EventAreaId, HttpContext.Request.Cookies["secret_jwt_key"]);
            var eventInArea = await _eventClient.GetEventByIdAsync(eventArea.EventId, HttpContext.Request.Cookies["secret_jwt_key"]);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userClient.GetUserById(userId);
            var ticket = new TicketViewModel
            {
                EventSeatId = id,
                Price = eventArea.Price,
                UserId = userId,
                DateOfPurchase = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(user.TimeZoneId)),
                Row = eventSeat.Row,
                Number = eventSeat.Number,
                UserName = user.UserName,
                EventName = eventInArea.Name,
                EventDateStart = eventInArea.DateStart,
                EventDateEnd = eventInArea.DateEnd,
                UserBalance = user.Balance,
            };
            return View(ticket);
        }

        /// <summary>
        /// Method for create ticket data.
        /// </summary>
        /// <param name="ticket">Object of ticket.</param>
        /// <returns>redirect to action.</returns>
        [HttpPost]
        [ValidationExceptionFilter]
        public async Task<IActionResult> Create(TicketViewModel ticket)
        {
            var ticketDto = new TicketDto
            {
                DateOfPurchase = ticket.DateOfPurchase,
                EventSeatId = ticket.EventSeatId,
                Price = ticket.Price,
                UserId = ticket.UserId,
            };
            var user = await _userClient.GetUserById(ticket.UserId);
            user.Balance -= ticket.Price;
            await _userClient.UpdateAsync(user);
            await _ticketClient.AddTicketAsync(ticketDto, HttpContext.Request.Cookies["secret_jwt_key"]);
            var seat = await _eventClient.GetEventSeatByIdAsync(ticket.EventSeatId, HttpContext.Request.Cookies["secret_jwt_key"]);
            var area = await _eventClient.GetEventAreaByIdAsync(seat.EventAreaId, HttpContext.Request.Cookies["secret_jwt_key"]);
            return Redirect($"~/EventSeat/Index/{area.Id}");
        }

        /// <summary>
        /// Method for delete tcket data.
        /// </summary>
        /// <param name="id">ticket id.</param>
        /// <returns>view result.</returns>
        [Authorize(Roles = Role.User + ", " + Role.Admin + ", " + Role.EventManager)]
        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var ticket = await _ticketClient.GetTicketByIdAsync(id, HttpContext.Request.Cookies["secret_jwt_key"]);
            var ticketToDelete = new TicketViewModel
            {
                Id = ticket.Id,
                DateOfPurchase = ticket.DateOfPurchase,
                Price = ticket.Price,
            };
            return View(ticketToDelete);
        }

        /// <summary>
        /// Method for delete tcket data.
        /// </summary>
        /// <param name="id">ticket id.</param>
        /// <returns>redirect.</returns>
        [HttpPost]
        [ValidationExceptionFilter]
        public async Task<IActionResult> Delete(int id)
        {
            var ticket = await _ticketClient.GetTicketByIdAsync(id, HttpContext.Request.Cookies["secret_jwt_key"]);
            var user = await _userClient.GetUserById(ticket.UserId);
            var seat = await _eventClient.GetEventSeatByIdAsync(ticket.EventSeatId, HttpContext.Request.Cookies["secret_jwt_key"]);
            var area = await _eventClient.GetEventAreaByIdAsync(seat.EventAreaId, HttpContext.Request.Cookies["secret_jwt_key"]);
            user.Balance += ticket.Price;
            await _ticketClient.DeleteTicketAsync(id, HttpContext.Request.Cookies["secret_jwt_key"]);
            await _userClient.UpdateAsync(user);
            return Redirect($"~/EventSeat/Index/{area.Id}");
        }
    }
}
