using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestEase;
using TicketManagement.Presentation.IdentityData;

namespace TicketManagement.Presentation.Client
{
    /// <summary>
    /// Interface for user rest client.
    /// </summary>
    public interface IUserRestClient
    {
        /// <summary>
        /// Method for registered.
        /// </summary>
        /// <param name="content">Register model.</param>
        /// <returns>redirect to action.</returns>
        [Post("account/register")]
        public Task<string> Register([Body] Register content);

        /// <summary>
        /// Method for log in.
        /// </summary>
        /// <param name="content">model for login.</param>
        /// <returns>view result.</returns>
        [Post("account/login")]
        public Task<AuthenticateResponse> Login([Body] Login content);

        /// <summary>
        /// Method for find user by name.
        /// </summary>
        /// <param name="userName">user name.</param>
        /// <returns>action result.</returns>
        [Get("account/FindUserByName")]
        public Task<User> FindUserByName(string userName);

        /// <summary>
        /// Method for update user data.
        /// </summary>
        /// <param name="user">user.</param>
        /// <returns>action result.</returns>
        [Put("account/Update")]
        public Task<IActionResult> UpdateAsync([Body] User user);

        /// <summary>
        /// Method for get all roles.
        /// </summary>
        /// <returns>roles.</returns>
        [Get("role/GetAllRoles")]
        public Task<List<string>> GetAllRoles();

        /// <summary>
        /// Method for get all users.
        /// </summary>
        /// <returns>users.</returns>
        [Get("role/GetAllUsers")]
        public Task<List<User>> GetAllUsers();

        /// <summary>
        /// Method for get role by id.
        /// </summary>
        /// <param name="userId">user id.</param>
        /// <returns>user.</returns>
        [Get("role/GetUserById")]
        public Task<User> GetUserById(string userId);

        /// <summary>
        /// Method for get user roles.
        /// </summary>
        /// <param name="userId">user id.</param>
        /// <returns>roles.</returns>
        [Get("role/GetUserRoleById")]
        public Task<List<string>> GetUserRoleById(string userId);

        /// <summary>
        /// Method for get roles.
        /// </summary>
        /// <param name="user">user.</param>
        /// <returns>roles.</returns>
        [Get("role/GetUserRoles")]
        public Task<IList<string>> GetUserRoles([Body] User user);

        /// <summary>
        /// Methid for delete user roles.
        /// </summary>
        /// <param name="roles">role to delete.</param>
        /// <param name="token">token.</param>
        /// <returns>action result.</returns>
        [Delete("role/DeleteUserRoles")]
        public Task<IActionResult> DeleteUserRoles([Body] IEnumerable<string> roles, [Header("Authorization")] string token);

        /// <summary>
        /// method for add user.
        /// </summary>
        /// <param name="model">user model.</param>
        /// <param name="token">token.</param>
        /// <returns>action result.</returns>
        [Post("role/AddUserToRoles")]
        public Task<IActionResult> AddUserToRoles([Body] UserRolesModel model, [Header("Authorization")] string token);

        /// <summary>
        /// method for remove user roles.
        /// </summary>
        /// <param name="model">user model.</param>
        /// <param name="token">token.</param>
        /// <returns>action result.</returns>
        [Delete("role/RemoveUserRoles")]
        public Task<IActionResult> RemoveUserRoles([Body] UserRolesModel model, [Header("Authorization")] string token);

        [Get("account/validate")]
        public Task ValidateToken(string token);

        /// <summary>
        /// Method for delete user.
        /// </summary>
        /// <param name="user">user.</param>
        /// <returns>truthfulness of delte.</returns>
        [Delete("account/Delete")]
        public Task<IActionResult> DeleteAsync([Body] User user);

        /// <summary>
        /// Method for add user.
        /// </summary>
        /// <param name="model">password model.</param>
        /// <returns>action result.</returns>
        [Post("account/AddUser")]
        public Task<IActionResult> AddUser([Body] ChangePassword model);

        /// <summary>
        /// Method for change user password.
        /// </summary>
        /// <param name="model">password model.</param>
        /// <returns>action result.</returns>
        [Post("account/ChangePassword")]
        public Task<IActionResult> ChangePassword([Body] ChangePassword model);
    }
}