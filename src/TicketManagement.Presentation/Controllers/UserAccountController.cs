using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using TicketManagement.Presentation.Client;
using TicketManagement.Presentation.Filters;
using TicketManagement.Presentation.IdentityData;
using TicketManagement.Presentation.Models;
using TicketManagement.Presentation.RoleData;
using TicketManagement.Presentation.Settings;

namespace TicketManagement.Presentation.Controllers
{
    /// <summary>
    /// Controller for work with users accounts.
    /// </summary>
    [Authorize(Roles = Role.User)]
    [FeatureGate(FeatureFlags.PresentationUI)]
    public class UserAccountController : Controller
    {
        private readonly IEventRestClient _eventRestClient;
        private readonly ITicketRestClient _ticketRestClient;
        private readonly IUserRestClient _userRestClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAccountController"/> class.
        /// Class constructor.
        /// </summary>
        /// <param name="userRestClient">user client.</param>
        /// <param name="eventRestClient">event client.</param>
        /// <param name="ticketRestClient">ticket client.</param>
        public UserAccountController(IEventRestClient eventRestClient, ITicketRestClient ticketRestClient, IUserRestClient userRestClient)
        {
            _eventRestClient = eventRestClient;
            _ticketRestClient = ticketRestClient;
            _userRestClient = userRestClient;
        }

        /// <summary>
        /// Method for select user data.
        /// </summary>
        /// <returns>view result.</returns>
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userRestClient.GetUserById(userId);
            var userInformation = new EditUser
            {
                Id = userId,
                Balance = user.Balance,
                Email = user.Email,
                Language = user.Language,
                Name = user.Name,
                Patronymic = user.Patronymic,
                Surname = user.Surname,
                TimeZoneId = user.TimeZoneId,
                Year = user.Year,
            };
            return View(userInformation);
        }

        /// <summary>
        /// Method for add money in user account.
        /// </summary>
        /// <param name="id">user id.</param>
        /// <returns>view result.</returns>
        public async Task<IActionResult> AddMoney(string id)
        {
            var user = await _userRestClient.GetUserById(id);
            var userInformation = new EditUser
            {
                Id = id,
                Balance = user.Balance,
            };
            return View(userInformation);
        }

        /// <summary>
        /// Method for add money in user account.
        /// </summary>
        /// <param name="editUser">user.</param>
        /// <returns>redirect to action.</returns>
        [HttpPost]
        public async Task<IActionResult> AddMoney(EditUser editUser)
        {
            var user = await _userRestClient.GetUserById(editUser.Id);
            user.Balance += editUser.Balance;
            await _userRestClient.UpdateAsync(user);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Method for edit user account data.
        /// </summary>
        /// <param name="id">user account id.</param>
        /// <returns>view result.</returns>
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userRestClient.GetUserById(id);
            var userInformation = new EditUser
            {
                Id = id,
                Balance = user.Balance,
                Email = user.Email,
                Language = user.Language,
                Name = user.Name,
                Patronymic = user.Patronymic,
                Surname = user.Surname,
                TimeZoneId = user.TimeZoneId,
                Year = user.Year,
            };
            return View(userInformation);
        }

        /// <summary>
        /// Method for edit user data.
        /// </summary>
        /// <param name="editUser">Object of user account.</param>
        /// <returns>redirect to action.</returns>
        [HttpPost]
        [ValidationExceptionFilter]
        public async Task<IActionResult> Edit(EditUser editUser)
        {
            var user = await _userRestClient.GetUserById(editUser.Id);
            user.Email = editUser.Email;
            user.Name = editUser.Name;
            user.Language = editUser.Language;
            user.TimeZoneId = editUser.TimeZoneId;
            user.Year = editUser.Year;
            user.Patronymic = editUser.Patronymic;
            user.Surname = editUser.Surname;

            await _userRestClient.UpdateAsync(user);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Method for get user purchase history.
        /// </summary>
        /// <param name="id">ticket id.</param>
        /// <returns>view result.</returns>
        public async Task<IActionResult> SeePurchaseHistory(string id)
        {
            var tickets = await _ticketRestClient.GetTicketByParentStringIdAsync(id, HttpContext.Request.Cookies["secret_jwt_key"]);
            var ticketsModel = new List<TicketViewModel>();
            foreach (var ticketModel in tickets)
            {
                var seat = await _eventRestClient.GetEventSeatByIdAsync(ticketModel.EventSeatId, HttpContext.Request.Cookies["secret_jwt_key"]);
                var area = await _eventRestClient.GetEventAreaByIdAsync(seat.EventAreaId, HttpContext.Request.Cookies["secret_jwt_key"]);
                var eventInArea = await _eventRestClient.GetEventByIdAsync(area.EventId, HttpContext.Request.Cookies["secret_jwt_key"]);
                var user = await _userRestClient.GetUserById(ticketModel.UserId);
                ticketsModel.Add(new TicketViewModel
                {
                    Id = ticketModel.Id,
                    Price = ticketModel.Price,
                    DateOfPurchase = ticketModel.DateOfPurchase,
                    Number = seat.Number,
                    Row = seat.Row,
                    UserName = user.Email,
                    EventDateEnd = eventInArea.DateEnd,
                    EventDateStart = eventInArea.DateStart,
                    EventName = eventInArea.Name,
                });
            }

            return View(ticketsModel);
        }
    }
}
