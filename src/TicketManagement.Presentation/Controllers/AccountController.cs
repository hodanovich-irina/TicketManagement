using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using TicketManagement.Presentation.Client;
using TicketManagement.Presentation.IdentityData;

namespace TicketManagement.Presentation.Controllers
{
    /// <summary>
    /// Controller for work with accounts.
    /// </summary>.
    public class AccountController : Controller
    {
        private readonly IStringLocalizer<AccountController> _localizer;
        private readonly IUserRestClient _userRestClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// Class constructor.
        /// </summary>
        /// <param name="localizer">localizer.</param>
        /// <param name="userRestClient">user rest client.</param>
        public AccountController(IStringLocalizer<AccountController> localizer,
            IUserRestClient userRestClient)
        {
            _localizer = localizer;
            _userRestClient = userRestClient;
        }

        /// <summary>
        /// Method for registered.
        /// </summary>
        /// <returns>view result.</returns>
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Method for registered.
        /// </summary>
        /// <param name="model">RegisterViewModel object.</param>
        /// <returns>redirect to action.</returns>
        [HttpPost]
        public async Task<IActionResult> Register(Register model)
        {
            var token = await _userRestClient.Register(model);
            HttpContext.Response.Cookies.Append("secret_jwt_key", "Bearer " + token, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
            });
            var login = new Login
            {
                Email = model.Email,
                Password = model.Password,
            };
            await Login(login);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Method for log in.
        /// </summary>
        /// <param name="returnUrl">Url.</param>
        /// <returns>view result.</returns>
        [HttpGet("login")]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new Login { ReturnUrl = returnUrl });
        }

        /// <summary>
        /// Method for log in.
        /// </summary>
        /// <param name="model">LoginViewModel object.</param>
        /// <returns>redirect to action.</returns>
        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] Login model)
        {
            var token = await _userRestClient.Login(model);
            HttpContext.Response.Cookies.Append("secret_jwt_key", "Bearer " + token.Token, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
            });
            var jwtSecurity = new JwtSecurityTokenHandler().ReadJwtToken(token.Token);
            var user = GetIdentityUser(token);
            var userI = await _userRestClient.FindUserByName(jwtSecurity.Payload.Sub);
            var roles = await _userRestClient.GetUserRoles(userI);
            var userModel = new UserRolesModel
            {
                User = userI,
                Roles = roles,
            };

            if (token.Token != null)
            {
                await Authenticate(userModel);
                HttpContext.Response.Cookies.Append("timeZone", user.TimeZoneId);
                HttpContext.Response.Cookies.Append("user_name", user.Email);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", _localizer["ErrorLoginPassword"]);
            }

            return View(model);
        }

        /// <summary>
        /// Method for log out.
        /// </summary>
        /// <returns>redirect to action.</returns>
        [HttpPost("logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Response.Cookies.Delete("secret_jwt_key");
            HttpContext.Response.Cookies.Delete("tokenToReact");
            HttpContext.Response.Cookies.Delete("user_name");
            HttpContext.Response.Cookies.Delete("userToReact");
            return RedirectToAction("Index", "Home");
        }

        private User GetIdentityUser(AuthenticateResponse user)
        {
            var identityUser = new User
            {
                Id = user.Id,
                Surname = user.Surname,
                Name = user.Name,
                Email = user.UserName,
                UserName = user.UserName,
                Patronymic = user.Patronymic,
                Year = user.Year,
                TimeZoneId = user.TimeZoneId,
                Balance = user.Balance,
                Language = user.Language,
            };

            return identityUser;
        }

        private async Task Authenticate(UserRolesModel user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.User.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.User.Id),
            };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role));
            }

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
