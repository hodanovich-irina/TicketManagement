using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace TicketManagement.Presentation.Filters
{
    /// <summary>
    /// Class for exception filter.
    /// </summary>
    public class ValidationExceptionFilterAttribute : Attribute, IExceptionFilter
    {
        /// <summary>
        /// Method for catching exception.
        /// </summary>
        /// <param name="context">exception cintext.</param>
        public void OnException(ExceptionContext context)
        {
            var obj = context.Exception.GetType().GetProperty("Content").GetValue(context.Exception);
            string message = (string)obj;
            string exceptionMessage = message;

            context.ExceptionHandled = true;
            var buffer = $"{exceptionMessage}";
            context.HttpContext.Response.Cookies.Append("message", buffer);
            context.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "controller", "Home" },
                    { "action", "ErrorMessage" },
                    { "message", buffer },
                });
        }
    }
}
