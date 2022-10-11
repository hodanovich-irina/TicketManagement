using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Presentation.Client;
using TicketManagement.Presentation.IdentityData;
using TicketManagement.Presentation.RoleData;

namespace TicketManagement.Presentation.Controllers
{
    /// <summary>
    /// Controller for work with roles.
    /// </summary>
    [Authorize(Roles = Role.Admin + ", " + Role.VenueManager)]
    public class RoleController : Controller
    {
        private readonly IUserRestClient _userRestClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleController"/> class.
        /// Class constructor.
        /// </summary>
        /// <param name="userRestClient">.</param>
        public RoleController(IUserRestClient userRestClient)
        {
            _userRestClient = userRestClient;
        }

        /// <summary>
        /// Method for show all roles.
        /// </summary>
        /// <returns>view result.</returns>
        public async Task<IActionResult> Index() => View(await _userRestClient.GetAllRoles());

        /// <summary>
        /// Method for delete role.
        /// </summary>
        /// <param name="id">role id.</param>
        /// <returns>redirect to action.</returns>
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _userRestClient.GetUserRoleById(id);
            if (role != null)
            {
                await _userRestClient.DeleteUserRoles(role, HttpContext.Request.Cookies["secret_jwt_key"]);
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Method for get users roles.
        /// </summary>
        /// <returns>view result.</returns>
        public async Task<IActionResult> UserList() => View(await _userRestClient.GetAllUsers());

        public async Task<IActionResult> Edit(string userId)
        {
            var user = await _userRestClient.GetUserById(userId);
            if (user != null)
            {
                var userRoles = await _userRestClient.GetUserRoles(user);
                var allRoles = await _userRestClient.GetAllRoles();
                var model = new ChangeRole
                {
                    UserId = user.Id,
                    UserEmail = user.UserName,
                    UserRoles = userRoles,
                    AllRoles = allRoles,
                };
                return View(model);
            }

            return NotFound();
        }

        /// <summary>
        /// Method for edit user role.
        /// </summary>
        /// <param name="userId">user id.</param>
        /// <param name="roles">role.</param>
        /// <returns>redirect.</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            var user = await _userRestClient.GetUserById(userId);
            if (user != null)
            {
                var userRoles = await _userRestClient.GetUserRoles(user);
                var addedRoles = roles.Except(userRoles);
                var removedRoles = userRoles.Except(roles);
                var modelToAdd = new UserRolesModel
                {
                    Roles = addedRoles,
                    User = user,
                };
                var modelToRemove = new UserRolesModel
                {
                    Roles = removedRoles,
                    User = user,
                };
                await _userRestClient.AddUserToRoles(modelToAdd, HttpContext.Request.Cookies["secret_jwt_key"]);
                await _userRestClient.RemoveUserRoles(modelToRemove, HttpContext.Request.Cookies["secret_jwt_key"]);
                return RedirectToAction("UserList");
            }

            return NotFound();
        }
    }
}
