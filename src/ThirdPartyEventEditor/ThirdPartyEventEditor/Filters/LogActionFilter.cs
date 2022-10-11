using System.Diagnostics;
using System.IO;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;

namespace ThirdPartyEventEditor.Filters
{
    /// <summary>
    /// Class for logging.
    /// </summary>
    public class LogActionFilter : ActionFilterAttribute, IActionFilter
    {
        private readonly Stopwatch _stopWatch;

        public LogActionFilter()
        {
            _stopWatch = new Stopwatch();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _stopWatch.Start();
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            _stopWatch.Stop();
            Log(filterContext.RouteData, _stopWatch.ElapsedMilliseconds);
        }

        private void Log(RouteData routeData, long time)
        {
            var controllerName = routeData.Values["controller"];
            var actionName = routeData.Values["action"];
            var text = "Time: " + time.ToString() + "ms" + " controller: "+ controllerName.ToString() + " action: " + actionName + "\n";
            var writePath = HostingEnvironment.MapPath(WebConfigurationManager.AppSettings["LoggerFile"]);
            if (!File.Exists(writePath)) 
            {
                File.WriteAllText(writePath,"");
            }
            File.AppendAllText(writePath, text);
        }
    }
}