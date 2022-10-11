using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.UserAPI.Dto;
using TicketManagement.UserAPI.Initializers;
using TicketManagement.UserAPI.Services;

namespace TicketManagement.UserAPI.Controllers
{
    /// <summary>
    /// Controller for work with accounts.
    /// </summary>.
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<UserDto> _userManager;
        private readonly SignInManager<UserDto> _signInManager;
        private readonly JwtTokenService _jwtTokenService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// Class constructor.
        /// </summary>
        /// <param name="userManager">user manager.</param>
        /// <param name="signInManager">sign in manager.</param>
        /// <param name="jwtTokenService">.</param>
        public AccountController(UserManager<UserDto> userManager, SignInManager<UserDto> signInManager, JwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
        }

        /// <summary>
        /// Method for registered.
        /// </summary>
        /// <param name="model">Register model.</param>
        /// <returns>redirect to action.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var user = new UserDto
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name,
                Year = model.Year,
                Surname = model.Surname,
                Patronymic = model.Patronymic,
                Language = model.Language,
                TimeZoneId = model.TimeZoneId,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            await _userManager.AddToRoleAsync(user, "user");
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                var roles = await _userManager.GetRolesAsync(user);
                return Ok(_jwtTokenService.GetToken(user, roles));
            }

            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Method for log in.
        /// </summary>
        /// <param name="model">model for login.</param>
        /// <returns>view result.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                var roles = await _userManager.GetRolesAsync(user);
                var token = _jwtTokenService.GetToken(user, roles);
                var response = new AuthenticateResponse(user, token);
                return Ok(response);
            }

            return Forbid();
        }

        /// <summary>
        /// Method for update user data.
        /// </summary>
        /// <param name="user">user.</param>
        /// <returns>action result.</returns>
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync([FromBody] UserDto user)
        {
            try
            {
                var userI = await _userManager.FindByIdAsync(user.Id);
                userI.Id = user.Id;
                userI.Balance = user.Balance;
                userI.Email = user.Email;
                userI.Language = user.Language;
                userI.Name = user.Name;
                userI.Patronymic = user.Patronymic;
                userI.Surname = user.Surname;
                userI.TimeZoneId = user.TimeZoneId;
                userI.Year = user.Year;
                await _userManager.UpdateAsync(userI);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Method for update user balance.
        /// </summary>
        /// <param name="user">user.</param>
        /// <returns>action result.</returns>
        [HttpPut("UpdateBalance")]
        public async Task<IActionResult> UpdateBalanceAsync([FromBody] UserDto user)
        {
            try
            {
                var userI = await _userManager.FindByIdAsync(user.Id);
                userI.Balance += user.Balance;
                await _userManager.UpdateAsync(userI);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetCurrentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var name = User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByNameAsync(name);
            return Ok(user);
        }

        [HttpGet("validate")]
        public IActionResult Validate(string token)
        {
            return _jwtTokenService.ValidateToken(token) ? Ok(User.Identity.Name) : (IActionResult)Forbid();
        }

        /// <summary>
        /// Method for find user by name.
        /// </summary>
        /// <param name="userName">user name.</param>
        /// <returns>action result.</returns>
        [HttpGet("FindUserByName")]
        public async Task<IActionResult> FindUserByName([FromQuery] string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return Ok(user);
        }

        /// <summary>
        /// Method for sign in.
        /// </summary>
        /// <param name="user">user.</param>
        /// <returns>action result.</returns>
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] UserDto user)
        {
            try
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Method for log out.
        /// </summary>
        /// <returns>redirect to action.</returns>
        [HttpPost("SignOut")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        /// <summary>
        /// Method for delete user.
        /// </summary>
        /// <param name="user">user.</param>
        /// <returns>action result.</returns>
        [HttpDelete("Delete")]
        [Authorize(Roles = Role.Admin + ", " + Role.VenueManager)]
        public async Task<IActionResult> DeleteAsync([FromBody] UserDto user)
        {
            try
            {
                var userI = await _userManager.FindByIdAsync(user.Id);
                await _userManager.DeleteAsync(userI);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Method for add user.
        /// </summary>
        /// <param name="model">password model.</param>
        /// <returns>action result.</returns>
        [HttpPost("AddUser")]
        [Authorize(Roles = Role.Admin + ", " + Role.VenueManager)]
        public async Task<IActionResult> AddUser([FromBody] ChangePassword model)
        {
            try
            {
                await _userManager.CreateAsync(model.User, model.NewPassword);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Method for change user password.
        /// </summary>
        /// <param name="model">password model.</param>
        /// <returns>action result.</returns>
        [HttpPost("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePassword model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(model.User.Id);
                await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
