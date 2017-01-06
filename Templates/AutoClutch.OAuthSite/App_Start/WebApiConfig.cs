using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
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
            config.Count().Filter().OrderBy().Expand().Select().MaxTop(null); //new line

            builder.EntitySet<blogEntry>("blogEntries");
            // http://stackoverflow.com/questions/36344979/odata-include-custom-properties-added-to-entity-framework-models-via-partial-c
            // http://stackoverflow.com/questions/27277306/odata-read-only-property
            builder.StructuralTypes.First(x => x.ClrType.FullName.Contains("blogEntry"))
                .AddProperty((typeof(blogEntry)).GetProperty("blogBodySummaryHtml"));

            builder.StructuralTypes.First(x => x.ClrType.FullName.Contains("blogEntry"))
                .AddProperty((typeof(blogEntry)).GetProperty("monthAbbreviation"));

            builder.StructuralTypes.First(x => x.ClrType.FullName.Contains("blogEntry"))
                .AddProperty((typeof(blogEntry)).GetProperty("day"));

            builder.StructuralTypes.First(x => x.ClrType.FullName.Contains("blogEntry"))
                .AddProperty((typeof(blogEntry)).GetProperty("year"));

            builder.EntitySet<author>("authors");

            builder.EntitySet<user>("users");

            builder.EntityType<user>().Collection
                .Function("GetLoggedInUser")
                .Returns<string>()
                .Namespace="usersService";

            config.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: "odata",
                model: builder.GetEdmModel());
        }
    }
}
