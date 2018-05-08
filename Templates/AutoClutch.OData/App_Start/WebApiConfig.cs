using $safeprojectname$.Core.Models;
using Microsoft.OData.Edm;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;

namespace $safeprojectname$
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // CORS configuration.
            var cors = new EnableCorsAttribute("http://lfkqbwtweb02", "*", "*");
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

            builder.EntitySet<contract>("contractsOData");

            // i.e. http://localhost/$safeprojectname$/odata/contractsOData/contractService.GetContractId(contractNumber='1374-GWN')
            var GetContractIdFunction = builder.EntityType<contract>().Collection.Function("GetContractId");
            GetContractIdFunction.Returns<int>();
            GetContractIdFunction.Parameter<string>("contractNumber");
            GetContractIdFunction.Namespace = "contractService";

            // i.e. http://localhost/$safeprojectname$/odata/contractsOData/contractService.GetInitialContract(sectionName='Mechanical Contracts')
            var GetInitialContractFunction = builder.EntityType<contract>().Collection.Function("GetInitialContract");
            GetInitialContractFunction.ReturnsFromEntitySet<contract>("contractsOData");
            GetInitialContractFunction.Parameter<string>("sectionName");
            GetInitialContractFunction.Namespace = "contractService";

            // http://stackoverflow.com/questions/22824723/how-to-make-a-property-nullable-in-web-api
            // Make model required properties optional in OData WebAPI.
            var contractNumberCollectionProperty = builder.EntityType<contract>().Property(i => i.contractNumber);
            contractNumberCollectionProperty.IsOptional();

            var contractDescriptionCollectionProperty = builder.EntityType<contract>().Property(i => i.contractDescription);
            contractDescriptionCollectionProperty.IsOptional();

            // https://docs.microsoft.com/en-us/aspnet/web-api/overview/odata-support-in-aspnet-web-api/odata-v4/odata-actions-and-functions
            var getDashboardMetric = builder.Function("GetDashboardMetric");
            getDashboardMetric.Returns<IHttpActionResult>();
            getDashboardMetric.Parameter<string>("name");
            getDashboardMetric.Parameter<int?>("loggedInUserId");

            SetNotMappedTypes<contract>(builder);

            builder.EntitySet<engineer>("engineersOData");

            SetNotMappedTypes<engineer>(builder);

            builder.EntitySet<engineerContract>("engineerContractsOData");

            SetNotMappedTypes<engineerContract>(builder);

            builder.EntitySet<contractStatus>("contractStatusesOData");

            SetNotMappedTypes<contractStatus>(builder);

            builder.EntitySet<changeOrderType>("changeOrderTypesOData");

            SetNotMappedTypes<changeOrderType>(builder);

            builder.EntitySet<changeOrder>("changeOrdersOData");

            SetNotMappedTypes<changeOrder>(builder);

            builder.EntitySet<contractType>("contractTypesOData");

            SetNotMappedTypes<contractType>(builder);

            builder.EntitySet<contractCategory>("contractCategoriesOData");

            SetNotMappedTypes<contractCategory>(builder);

            builder.EntitySet<reportingCategory>("reportingCategoriesOData");

            SetNotMappedTypes<reportingCategory>(builder);

            builder.EntitySet<receivingReportDetail>("receivingReportDetailsOData");

            SetNotMappedTypes<receivingReportDetail>(builder);

            builder.EntitySet<objectCode>("objectCodesOData");

            SetNotMappedTypes<objectCode>(builder);

            builder.EntitySet<budgetCode>("budgetCodesOData");

            SetNotMappedTypes<budgetCode>(builder);

            builder.EntitySet<payment>("paymentsOData");

            SetNotMappedTypes<payment>(builder);

            builder.EntitySet<contractor>("contractorsOData");

            SetNotMappedTypes<contractor>(builder);

            builder.EntitySet<contractorContactPerson>("contractorContactPersonsOData");

            SetNotMappedTypes<contractorContactPerson>(builder);

            builder.EntitySet<deductionType>("deductionTypesOData");

            SetNotMappedTypes<deductionType>(builder);

            builder.EntitySet<section>("sectionsOData");

            SetNotMappedTypes<section>(builder);

            builder.EntitySet<location>("locationsOData");

            SetNotMappedTypes<location>(builder);

            builder.EntitySet<deduction>("deductionsOData");

            SetNotMappedTypes<deduction>(builder);

            builder.EntitySet<workOrderStatus>("workOrderStatusesOData");

            SetNotMappedTypes<workOrderStatus>(builder);

            builder.EntitySet<workOrder>("workOrdersOData");

            SetNotMappedTypes<workOrder>(builder);

            builder.EntitySet<serviceType>("serviceTypesOData");

            SetNotMappedTypes<serviceType>(builder);

            builder.EntitySet<repairType>("repairTypesOData");

            SetNotMappedTypes<repairType>(builder);

            builder.EntitySet<workOrderHistory>("workOrderHistoriesOData");

            SetNotMappedTypes<workOrderHistory>(builder);

            return builder.GetEdmModel();
        }

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
