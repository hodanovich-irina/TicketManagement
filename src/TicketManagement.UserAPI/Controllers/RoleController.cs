using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.UserAPI.Dto;
using TicketManagement.UserAPI.Initializers;

namespace TicketManagement.UserAPI.Controllers
{
    /// <summary>
    /// Controller for work with roles.
    /// </summary>
    [Route("[controller]")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<UserDto> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleController"/> class.
        /// Class constructor.
        /// </summary>
        /// <param name="roleManager">role manager.</param>
        /// <param name="userManager">user manager.</param>
        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<UserDto> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        /// <summary>
        /// methid for get all users roles.
        /// </summary>
        /// <returns>all roles.</returns>
        [HttpGet("GetAllRoles")]
        public async Task<List<string>> GetAllRoles()
        {
            var roles = await Task.Run(() => _roleManager.Roles);
            var rolesName = roles.Select(c => c.Name).ToList();
            return rolesName;
        }

        /// <summary>
        /// method for get all users.
        /// </summary>
        /// <returns>users.</returns>
        [HttpGet("GetAllUsers")]
        public async Task<List<UserDto>> GetAllUsers()
        {
            var users = await Task.Run(() => _userManager.Users.ToList());
            return users;
        }

        /// <summary>
        /// method for get user by id.
        /// </summary>
        /// <param name="userId">user id.</param>
        /// <returns>user.</returns>
        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return Ok(user);
        }

        /// <summary>
        /// method for get user role by id.
        /// </summary>
        /// <param name="userId">user id.</param>
        /// <returns>user role.</returns>
        [HttpGet("GetUserRoleById")]
        public async Task<IActionResult> GetUserRoleById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userRole = await _userManager.GetRolesAsync(user);
            return Ok(userRole);
        }

        /// <summary>
        /// method for get all user roles.
        /// </summary>
        /// <param name="user">user.</param>
        /// <returns>roles.</returns>
        [HttpGet("GetUserRoles")]
        public async Task<IActionResult> GetUserRoles([FromBody] UserDto user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }

        /// <summary>
        /// method for get all current user roles.
        /// </summary>
        /// <returns>roles.</returns>
        [HttpGet("GetCurrentUserRoles")]
        public async Task<IActionResult> GetCurrentUserRoles()
        {
            var name = User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByNameAsync(name);
            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }

        /// <summary>
        /// Methid for delete user roles.
        /// </summary>
        /// <param name="roles">role to delete.</param>
        /// <returns>action result.</returns>
        [HttpDelete("DeleteUserRoles")]
        [Authorize(Roles = Role.Admin + ", " + Role.VenueManager)]
        public async Task<IActionResult> DeleteUserRoles([FromBody] IEnumerable<string> roles)
        {
            foreach (var role in roles)
            {
                var roleToDelete = await _roleManager.FindByNameAsync(role);
                await _roleManager.DeleteAsync(roleToDelete);
            }

            return Ok();
        }

        /// <summary>
        /// method for add user.
        /// </summary>
        /// <param name="model">user model.</param>
        /// <returns>action result.</returns>
        [HttpPost("AddUserToRoles")]
        [Authorize(Roles = Role.Admin + ", " + Role.VenueManager)]
        public async Task<IActionResult> AddUserToRoles([FromBody] UserRolesModel model)
        {
            var user = await _userManager.FindByIdAsync(model.User.Id);
            await _userManager.AddToRolesAsync(user, model.Roles);
            return Ok();
        }

        /// <summary>
        /// method for remove user roles.
        /// </summary>
        /// <param name="model">user model.</param>
        /// <returns>action result.</returns>
        [HttpDelete("RemoveUserRoles")]
        [Authorize(Roles = Role.Admin + ", " + Role.VenueManager)]
        public async Task<IActionResult> RemoveUserRoles([FromBody] UserRolesModel model)
        {
            var user = await _userManager.FindByIdAsync(model.User.Id);
            await _userManager.RemoveFromRolesAsync(user, model.Roles);
            return Ok();
        }
    }
}
