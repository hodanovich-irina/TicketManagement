using Autofac;
using Autofac.Integration.Mvc;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ThirdPartyEventEditor.Interfaces;
using ThirdPartyEventEditor.Models;
using ThirdPartyEventEditor.Repositories;
using ThirdPartyEventEditor.Services;
using ThirdPartyEventEditor.Validators;

namespace ThirdPartyEventEditor
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<ThirdPartyEventRepository>().As<IRepository<ThirdPartyEvent>>();
            builder.RegisterType<ThirdPartyEventFileToExportCreator>().As<IThirdPartyEventFileToExportCreator<ThirdPartyEvent>>();
            builder.RegisterType<ThirdPartyEventService>().As<IService<ThirdPartyEvent>>();
            builder.RegisterType<ThirdPartyEventValidator>().As<IValidator<ThirdPartyEvent>>();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly());

            IContainer container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}