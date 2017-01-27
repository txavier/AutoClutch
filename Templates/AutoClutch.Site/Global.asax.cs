using AutoClutch.Auto.Service.Interfaces;
using $safeprojectname$.CompositionRoot;
using $safeprojectname$.DependencyResolution;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace $safeprojectname$
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static IContainer container { get; private set; }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // http://cdroulers.com/blog/2015/03/03/structuremap-3-and-asp-net-web-api-2/
            //container = new Container(c => c.AddRegistry<DefaultRegistry>());
            container = IoC.Initialize();

            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), new StructureMapWebApiControllerActivator(container));

            // http://www.amescode.com/category/structuremap/
            // Allows setter injection in validation attributes.
            DataAnnotationsModelValidatorProvider.RegisterDefaultAdapterFactory((metadata, context, attribute) => new StructuremapDataAnnotationsModelValidator(metadata, context, attribute, container));

            // Allows setter injection in IValidatableObject viewModels.
            DataAnnotationsModelValidatorProvider.RegisterDefaultValidatableObjectAdapterFactory((metadata, context) => new StructureMapValidatableObjectAdapterFactory(metadata, context, container));
        }

        protected void Application_End()
        {
            container.Dispose();
        }
    }

}
