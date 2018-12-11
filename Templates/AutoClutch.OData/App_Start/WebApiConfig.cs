using Microsoft.OData.Edm;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OTPS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using OTPS.Core.Objects;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using TrackerEnabledDbContext.Common.Models;

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

            var createModIntra = builder.Action("CreateModIntra").Returns<List<string>>();
            createModIntra.Parameter<int>("id");
            createModIntra.Parameter<string>("Line");
            createModIntra.Parameter<decimal>("Mod_Amt");

            var deleteModIntra = builder.Function("DeleteModIntra").Returns<IHttpActionResult>();
            deleteModIntra.Parameter<int>("key");
            deleteModIntra.Parameter<string>("item");

            var updateModIntra = builder.Action("UpdateModIntra").Returns<IHttpActionResult>();
            updateModIntra.Parameter<int>("key");
            updateModIntra.Parameter<string>("item");
            updateModIntra.Parameter<string>("newItem");
            updateModIntra.Parameter<decimal>("newAmt");

            var deleteIntraByKey = builder.Function("DeleteIntraByKey").Returns<IHttpActionResult>();
            deleteIntraByKey.Parameter<int>("key");

            var submitModIntra = builder.Function("SubmitModIntra").Returns<IHttpActionResult>();
            submitModIntra.Parameter<int>("key");
            submitModIntra.Parameter<string>("remark");

            var addContract = builder.Action("addContract").Returns<int>();
            addContract.EntityParameter<Contract>(typeof(Contract).Name);

            var getContractAmounts = builder.Function("GetContractAmounts").Returns<ContractAmountData>();
            getContractAmounts.Parameter<int>("id");
            getContractAmounts.Parameter<string>("start");
            getContractAmounts.Parameter<string>("end");

            var GetRegContract = builder.Function("GetRegContract").Returns<ContractSummaryData>();
            GetRegContract.Parameter<string>("Reg_No");
            GetRegContract.Parameter<string>("start");
            GetRegContract.Parameter<string>("end");

            var GetPinContract = builder.Function("GetPinContract").Returns<ContractSummaryData>();
            GetPinContract.Parameter<string>("Pin");
            GetPinContract.Parameter<string>("start");
            GetPinContract.Parameter<string>("end");

            var GetProjIDContract = builder.Function("GetProjIDContract").Returns<ContractSummaryData>();
            GetProjIDContract.Parameter<string>("ProjID");
            GetProjIDContract.Parameter<string>("start");
            GetProjIDContract.Parameter<string>("end");

            var GetObjectCodeContract = builder.Function("GetObjectCodeContract").Returns<ContractSummaryData>();
            GetObjectCodeContract.Parameter<string>("ObjectCode");
            GetObjectCodeContract.Parameter<string>("start");
            GetObjectCodeContract.Parameter<string>("end");

            var GetContractPayment = builder.Function("GetContractPayment").Returns<ContractSummaryPay>();
            GetObjectCodeContract.Parameter<string>("ProjNo");
            GetObjectCodeContract.Parameter<string>("ObjectCode");
            GetObjectCodeContract.Parameter<string>("Fstart");
            GetObjectCodeContract.Parameter<string>("Fend");
            GetObjectCodeContract.Parameter<string>("PStart");
            GetObjectCodeContract.Parameter<string>("PEnd");

            /*var GetTotal = builder.Function("GetTotal");
            GetTotal.Parameter<String>("filter");
            GetTotal.Returns<String[]>();*/



            //var getPermitsPerPlant = builder.Function("GetPermitsPerPlant");
            //getPermitsPerPlant.Returns<IHttpActionResult>();
            //getPermitsPerPlant.Returns<MetricsData>();

            //builder.EntitySet<user>("users");
            builder.EntitySet<Batch_RepCat>("Batch_RepCats");
            builder.EntitySet<Batch_RepCat_Budget>("Batch_RepCat_Budgets");
            builder.EntitySet<Bulk_Chemicals>("Bulk_Chemicals");
            builder.EntitySet<Contract>("Contracts");
            builder.EntitySet<Contract_History>("Contract_Historys");
            builder.EntitySet<contract_pay>("Contract_Pays");
            builder.EntitySet<contract_pay_history>("Contract_Pay_Historys");
            builder.EntitySet<CT_Mod_History>("CT_Mod_Historys");
            builder.EntitySet<Mod_Budget>("Mod_Budgets");
            builder.EntitySet<Mod_Intra>("Mod_Intras");
            builder.EntitySet<Mod_Logbook>("Mod_Logbooks");
            builder.EntitySet<Modi_Batch>("Modi_Batchs");
            EntitySetConfiguration<Modi_Loc> Modi_Loc = builder.EntitySet<Modi_Loc>("Modi_Locs");
            builder.EntitySet<Multi_Year_Grand>("Multi_Year_Grands");
            builder.EntitySet<Multi_Year_Po>("Multi_Year_Pos");
            builder.EntitySet<QAS_DATA>("QAS_Datas");
            builder.EntitySet<Ref_Document>("Ref_Documents");
            builder.EntitySet<Ref_ObjectAllocType>("Ref_ObjectAllocTypes");
            builder.EntitySet<Ref_ObjectChemicalCode>("Ref_ObjectChemicalCodes");
            builder.EntitySet<Ref_OrderType>("Ref_OrderTypes");
            builder.EntitySet<Ref_Vendor>("Ref_Vendors");
            builder.EntitySet<ReqList>("ReqLists");
            builder.EntitySet<ReqList_Budget>("ReqList_Budgets");
            builder.EntitySet<setting>("settings");
            builder.EntitySet<sp_BudgetCode>("sp_BudgetCodes");
            builder.EntitySet<sp_line>("sp_lines");
            EntitySetConfiguration<sp_object> sp_object = builder.EntitySet<sp_object>("sp_objects");
            builder.EntitySet<T_LOC>("T_LOCs");
            builder.EntitySet<T_Loc_Budget>("T_Loc_Budgets");
            builder.EntitySet<T_Loc_Share>("T_Loc_Shares");
            builder.EntitySet<T_Loc_Share_bak>("T_Loc_Share_baks");
            builder.EntitySet<T_Loc_Share_BK>("T_Loc_Share_BKs");
            builder.EntitySet<T_Rec>("T_Recs");
            builder.EntitySet<Track>("Tracks");
            builder.EntitySet<User>("Users");
            builder.EntitySet<AuditLog>("AuditLogs");

            var validate = sp_object.EntityType.Collection.Action("putByKeyString").Returns<Task<IHttpActionResult>>();
            validate.EntityParameter<sp_object>("entity");

            var validateModi_Loc = Modi_Loc.EntityType.Collection.Action("ModifyLoc").Returns<Task<IHttpActionResult>>();
            validateModi_Loc.EntityParameter<Modi_Loc>("entity");

            //SetNotMappedTypes<user>(builder);
            SetNotMappedTypes<Batch_RepCat>(builder);
            SetNotMappedTypes<Batch_RepCat_Budget>(builder);
            SetNotMappedTypes<Bulk_Chemicals>(builder);
            SetNotMappedTypes<Contract>(builder);
            SetNotMappedTypes<contract_pay>(builder);
            SetNotMappedTypes<contract_pay_history>(builder);
            SetNotMappedTypes<CT_Mod_History>(builder);
            SetNotMappedTypes<Mod_Budget>(builder);
            SetNotMappedTypes<Mod_Intra>(builder);
            SetNotMappedTypes<Mod_Logbook>(builder);
            SetNotMappedTypes<Modi_Batch>(builder);
            SetNotMappedTypes<Modi_Loc>(builder);
            SetNotMappedTypes<Multi_Year_Grand>(builder);
            SetNotMappedTypes<Multi_Year_Po>(builder);
            SetNotMappedTypes<QAS_DATA>(builder);
            SetNotMappedTypes<Ref_Document>(builder);
            SetNotMappedTypes<Ref_ObjectAllocType>(builder);
            SetNotMappedTypes<Ref_ObjectChemicalCode>(builder);
            SetNotMappedTypes<Ref_OrderType>(builder);
            SetNotMappedTypes<Ref_Vendor>(builder);
            SetNotMappedTypes<ReqList>(builder);
            SetNotMappedTypes<ReqList_Budget>(builder);
            SetNotMappedTypes<sp_BudgetCode>(builder);
            SetNotMappedTypes<sp_line>(builder);
            SetNotMappedTypes<sp_object>(builder);
            SetNotMappedTypes<setting>(builder);
            SetNotMappedTypes<T_LOC>(builder);
            SetNotMappedTypes<T_Loc_Budget>(builder);
            SetNotMappedTypes<T_Loc_Share>(builder);
            SetNotMappedTypes<T_Loc_Share_bak>(builder);
            SetNotMappedTypes<T_Loc_Share_BK>(builder);
            SetNotMappedTypes<T_Rec>(builder);
            SetNotMappedTypes<Track>(builder);
            SetNotMappedTypes<User>(builder);
            SetNotMappedTypes<AuditLog>(builder);


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
