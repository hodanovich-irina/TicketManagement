using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement.Mvc;

namespace TicketManagement.Presentation.Settings
{
    /// <summary>
    /// Class for generate a response.
    /// </summary>
    public class RedirectDisabledFeatureHandler : IDisabledFeaturesHandler
    {
        private readonly IConfiguration _configuration;

        public RedirectDisabledFeatureHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task HandleDisabledFeatures(IEnumerable<string> features, ActionExecutingContext context)
        {
            context.Result = new RedirectResult(_configuration["ReactUIPath"]);
            var lang = context.HttpContext.Request.Cookies[".AspNetCore.Culture"].Split("|");
            var langNow = string.Join("", lang[0].Skip(2).Take(lang[0].Length - 2));
            context.HttpContext.Response.Cookies.Append("language", langNow);
            if (context.HttpContext.Request.Cookies["secret_jwt_key"] != null)
            {
                context.HttpContext.Response.Cookies.Append("tokenToReact", context.HttpContext.Request.Cookies["secret_jwt_key"]);
                context.HttpContext.Response.Cookies.Append("userToReact", context.HttpContext.Request.Cookies["user_name"]);
            }

            return Task.CompletedTask;
        }
    }
}
