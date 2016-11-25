using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using $safeprojectname$.Core.Models;
using System.Web.Http.Dispatcher;

namespace $safeprojectname$
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Use camel case for JSON data.
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // New code:
            ODataModelBuilder builder = new ODataConventionModelBuilder();

            // http://odata.github.io/WebApi/#13-01-modelbound-attribute
            // http://stackoverflow.com/questions/39515218/odata-error-the-query-specified-in-the-uri-is-not-valid-the-property-cannot-be
            //config.Count().Filter().OrderBy().Expand().Select().MaxTop(null); //new line

            builder.EntitySet<actionFigure>("actionFigures");
            //builder.EntityType<actionFigure>().Filter("imageUrl");
            builder.EntityType<actionFigure>().Filter("name");
            builder.EntityType<actionFigure>().Select("name");

            config.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: "odata",
                model: builder.GetEdmModel());
        }
    }
}
