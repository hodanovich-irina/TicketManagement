using System;
using System.IO;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;

namespace ThirdPartyEventEditor.Filters
{
    /// <summary>
    /// Class for catch exception.
    /// </summary>
    public class ValidationExceptionFilterAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var userText = "Sorry, an error has occurred...";
            var text = "An exception occurred: " + filterContext.Exception.Message + " action: " + filterContext.RouteData.Values["action"].ToString() 
                + " controller: " + filterContext.RouteData.Values["controller"].ToString() + " time:" + DateTime.Now.ToString() + "\n";
            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "controller", "Home" },
                    { "action", "ShowError" },
                    { "message", userText },
                });
            filterContext.ExceptionHandled = true;

            var writePath = HostingEnvironment.MapPath(WebConfigurationManager.AppSettings["LoggerFile"]);
            if (!File.Exists(writePath))
            {
                File.WriteAllText(writePath, "");
            }
            File.AppendAllText(writePath, text);
        }
    }
}