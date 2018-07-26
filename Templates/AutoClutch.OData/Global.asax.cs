using StructureMap;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;

namespace $safeprojectname$
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        //public static IContainer container { get; private set; }

        protected void Application_Start()
        {
            System.Web.Mvc.AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);

            FilterConfig.RegisterGlobalFilters(System.Web.Mvc.GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }

}
