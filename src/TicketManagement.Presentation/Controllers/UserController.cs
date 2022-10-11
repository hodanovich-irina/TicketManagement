using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Presentation.Client;
using TicketManagement.Presentation.Filters;
using TicketManagement.Presentation.IdentityData;
using TicketManagement.Presentation.RoleData;

namespace TicketManagement.Presentation.Controllers
{
    /// <summary>
    /// Controller for work with users.
    /// </summary>
    public class UserController : Controller
    {
        private readonly IUserRestClient _userRestClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// Class constructor.
        /// </summary>
        /// <param name="userRestClient">.</param>
        public UserController(IUserRestClient userRestClient)
        {
            _userRestClient = userRestClient;
        }

        [Authorize(Roles = Role.Admin + ", " + Role.VenueManager)]
        public async Task<IActionResult> Index() => View(await _userRestClient.GetAllUsers());

        /// <summary>
        /// Method for create user data.
        /// </summary>
        /// <returns>view result.</returns>
        [Authorize(Roles = Role.Admin + ", " + Role.VenueManager)]
        public IActionResult Create() => View();

        /// <summary>
        /// Method for create user data.
        /// </summary>
        /// <param name="model">Object of user.</param>
        /// <returns>redirect to action.</returns>
        [HttpPost]
        [ValidationExceptionFilter]
        public async Task<IActionResult> Create(CreateUser model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Email = model.Email, UserName = model.Email, Year = model.Year, Language = model.Language,  Balance = model.Balance,
                    TimeZoneId = model.TimeZoneId, Surname = model.Surname, Patronymic = model.Patronymic, Name = model.Name,
                };
                var userToCreate = new ChangePassword
                {
                    User = user,
                    NewPassword = model.Password,
                };

                await _userRestClient.AddUser(userToCreate);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        /// <summary>
        /// Method for edit user data.
        /// </summary>
        /// <param name="id">user id.</param>
        /// <returns>view result.</returns>
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userRestClient.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new EditUser
            {
                Id = user.Id, Email = user.Email, Year = user.Year, Name = user.Name, Surname = user.Surname,
                TimeZoneId = user.TimeZoneId, Language = user.Language, Patronymic = user.Patronymic, Balance = user.Balance,
            };
            return View(model);
        }

        /// <summary>
        /// Method for edit user data.
        /// </summary>
        /// <param name="model">Object of user.</param>
        /// <returns>redirect to action.</returns>
        [HttpPost]
        [ValidationExceptionFilter]
        public async Task<IActionResult> Edit(EditUser model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRestClient.GetUserById(model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.Year = model.Year;
                    user.Balance = model.Balance;
                    user.Language = model.Language;
                    user.TimeZoneId = model.TimeZoneId;
                    user.Name = model.Name;
                    user.Surname = model.Surname;
                    user.Patronymic = model.Patronymic;

                    await _userRestClient.UpdateAsync(user);
                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }

        [Authorize(Roles = Role.Admin + ", " + Role.VenueManager)]

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            User user = await _userRestClient.GetUserById(id);
            if (user != null)
            {
                await _userRestClient.DeleteAsync(user);
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> ChangePassword(string id)
        {
            var user = await _userRestClient.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new ChangePassword { Id = user.Id, Email = user.Email };
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRestClient.GetUserById(model.Id);
                if (user != null)
                {
                    var changeModel = new ChangePassword
                    {
                        User = user,
                        NewPassword = model.NewPassword,
                        OldPassword = model.OldPassword,
                    };

                    await _userRestClient.ChangePassword(changeModel);
                    return Redirect("~/Home/Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
                }
            }

            return View(model);
        }
    }
}
