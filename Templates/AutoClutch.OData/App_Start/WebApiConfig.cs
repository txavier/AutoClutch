using Microsoft.OData.Edm;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using AutoClutchTemplate.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using AutoClutchTemplate.Core.Objects;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace $safeprojectname$
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // CORS configuration.
            var cors = new EnableCorsAttribute("*", "*", "*");
            cors.SupportsCredentials = true;

            config.EnableCors(cors);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;

            //json.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));

            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // http://stackoverflow.com/questions/11553760/why-does-json-net-deserializeobject-change-the-timezone-to-local-time/32278301#32278301
            json.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;

            config.Count().Filter().OrderBy().Expand().Select().MaxTop(null);

            config.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: "odata",
                model: GetEdmModel());

            //json.SerializerSettings.MaxDepth = 3;

            // Works, has the "$ref" only for arrays, however explicitly serializes all values.
            //json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            //json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Arrays;
        }

        private static IEdmModel GetEdmModel()
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();

            // https://docs.microsoft.com/en-us/aspnet/web-api/overview/odata-support-in-aspnet-web-api/odata-v4/odata-actions-and-functions
            var getDashboardMetric = builder.Function("GetDashboardMetric");
            getDashboardMetric.Returns<IHttpActionResult>();
            getDashboardMetric.Parameter<string>("name");
            getDashboardMetric.Parameter<int?>("loggedInUserId");

            var getLoggedInUser = builder.Function("GetLoggedInUser");
            getLoggedInUser.Returns<IHttpActionResult>();

            var concatenatePreviousFiles = builder.Function("ConcatenatePreviousFiles");
            concatenatePreviousFiles.Parameter<int>("projectId");
            concatenatePreviousFiles.Returns<IHttpActionResult>();

            var getTopFiveBudgetCodes = builder.Function("getTopFiveBudgetCodes");
            getTopFiveBudgetCodes.Returns<IHttpActionResult>();
            getTopFiveBudgetCodes.Returns<MetricsData>();


            var getByKeyString = builder.Function("getByKeyString");
            getByKeyString.Parameter<string>("Key");
            getByKeyString.Returns<IHttpActionResult>();

            var selectBudObj = builder.Function("SelectBudObj");
            selectBudObj.Returns<String[]>();

            //var getPermitsPerPlant = builder.Function("GetPermitsPerPlant");
            //getPermitsPerPlant.Returns<IHttpActionResult>();
            //getPermitsPerPlant.Returns<MetricsData>();

            //builder.EntitySet<user>("users");
            builder.EntitySet<setting>("settings");
            //builder.EntitySet<User>("Users");

            //SetNotMappedTypes<user>(builder);
            SetNotMappedTypes<setting>(builder);
            //SetNotMappedTypes<User>(builder);

            return builder.GetEdmModel();
        }


        /// <summary>
        /// This method tells odata that the properties that have custom attributes should be marked as 
        /// Not mapped so that odata doesnt try to evaluate them.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        private static void SetNotMappedTypes<T>(ODataModelBuilder builder)
        {
            var notMappedProperties = (typeof(T)).GetProperties().Where(i => i.CustomAttributes.Any(j => j.AttributeType.Name == "NotMappedAttribute"));

            var typeName = (typeof(T)).Name;

            foreach (var notMappedProperty in notMappedProperties)
            {
                builder.StructuralTypes.First(x => x.ClrType.Name.Equals(typeName))
                    .AddProperty(notMappedProperty);
            }
        }
    }
}
