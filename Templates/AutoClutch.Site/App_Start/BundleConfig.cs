using System.Web;
using System.Web.Optimization;

namespace $safeprojectname$
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                "~/Scripts/angular.min.js"
                , "~/Scripts/angular-route.min.js"
                , "~/Scripts/angular-animate.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/angular-third-party").Include(
                "~/Scripts/angular-ui/ui-bootstrap-tpls-0.14.2.min.js"
                , "~/Scripts/nya-bs-select.js"
                , "~/Scripts/loading-bar.min.js"
                , "~/Scripts/toaster.min.js"
                , "~/Scripts/jsnlog.min.js"
                , "~/Scripts/chartjs/chart.min.js"
                , "~/Scripts/angular-chart/angular-chart.min.js"
                , "~/Scripts/ng-file-upload.min.js"
                , "~/Scripts/ng-file-upload-shim.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/angular-third-party-datepicker-moment-shared-directives-moment-filter").Include(
                    "~/Scripts/moment.min.js"
                    , "~/Scripts/datetimepicker.js"
                    , "~/shared-directives/moment-filter/moment-filter.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/angular-app").Include(
                "~/app/app.module.js",
                "~/app/app.config.js",
                "~/app/services/data.service.js",
                "~/app/services/authentication.service.js",
                "~/app/services/jsonPointerParseService.js",
                "~/app/services/contract.service.js",
                "~/app/services/error.service.js",
                "~/app/services/report.service.js",
                "~/app/services/authorization.service.js",
                "~/app/services/document-upload.service.js",
                "~/app/home/home.controller.js",
                "~/app/sidebar/sidebar.controller.js",
                "~/app/contracts/contract.controller.js",
                "~/app/contracts/contracts.controller.js",
                "~/app/contracts/add-contract.controller.js",
                "~/app/payments/add-payment.controller.js",
                "~/app/payments/update-payment.controller.js",
                "~/app/deductions/add-deduction.controller.js",
                "~/app/change-orders/add-change-order.controller.js",
                "~/app/change-orders/update-change-order.controller.js",
                "~/app/specifications/specification.controller.js",
                "~/app/master-page/master-page.controller.js",
                "~/app/work-orders/work-orders.controller.js",
                "~/app/work-orders/add-work-order.controller.js",
                "~/app/work-orders/update-work-order.controller.js",
                "~/app/work-order-histories/work-order-histories.controller.js",
                "~/app/evaluations/add-evaluation.controller.js",
                "~/app/deductions/deductions.controller.js",
                "~/app/payments/payments.controller.js",
                "~/app/deductions/deduction.controller.js",
                "~/app/payments/payment-documents.controller.js",
                "~/app/histories/histories.controller.js",
                "~/app/histories/history.controller.js",
                "~/app/contracts/add-renewal-contract.controller.js",
                "~/app/kpi/kpi-report.controller.js",
                "~/app/contracts/contract-report.controller.js",
                "~/app/specifications/add-specification.controller.js",
                "~/app/payments/payment-pdf.controller.js",
                "~/app/engineers/engineer.controller.js",
                "~/app/engineers/engineers.controller.js",
                "~/app/contractors/contractors.controller.js",
                "~/app/contractors/add-contractor.controller.js",
                "~/app/contractors/update-contractor.controller.js",
                "~/app/contractor-contact-persons/add-contractor-contact-person.controller.js",
                "~/app/contractor-contact-persons/contractor-contact-persons.controller.js",
                "~/app/contractor-contact-persons/update-contractor-contact-person.controller.js",
                "~/app/user-action-logs/user-action-logs.controller.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/angular-shared-directives").Include(
                 "~/shared-directives/chart-angular/chartangular.js"
                , "~/shared-directives/autofocus/autofocus.js"
                , "~/shared-directives/shared-directives.js"
                , "~/shared-directives/back-button/back-button.js"
                //,"~/shared-directives/esri-map-geojson/esri-map-geojson.js"
                , "~/shared-directives/logToServer/logToServer.js"
                , "~/shared-directives/ng-really/ng-really.js"
                , "~/shared-directives/angular-autodisable/angular-autodisable.min.js"
                , "~/shared-directives/ng-print/ng-print.js"
                , "~/shared-directives/box-button/box-button.js"
                , "~/shared-directives/auto-input/auto-input.directive.js"
                , "~/shared-directives/focus-me/focus-me.js"
                , "~/shared-directives/blur-currency/blur-currency.js"
                // Solution specific directives.
                , "~/app/contracts/contract-details.directive.js"
                , "~/app/reports/report.directive.js"
                , "~/app/access/access.directive.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/sbAdmin").Include(
                "~/Scripts/plugins/morris/raphael-min.js",
                "~/Scripts/plugins/morris/morris.min.js",
                "~/Scripts/plugins/metisMenu/metisMenu.min.js",
                "~/Scripts/sb-admin-2.js"
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/toaster.min.css",
                "~/Content/loading-bar.min.css",
                "~/Content/ui-bootstrap-csp.css",
                "~/Content/bootstrap.min.css",
                "~/Content/nya-bs-select.min.css",
                "~/Content/plugins/metisMenu/metisMenu.min.css",
                "~/Content/timeline.css",
                "~/Content/sb-admin-2.css",
                "~/Content/plugins/morris.css",
                "~/fonts/font-awesome-4.6.1/css/font-awesome.min.css",
                "~/Content/datetimepicker.css",
                "~/shared-directives/ng-print/ng-print.css",
                "~/Content/angular-chart.css",
                "~/Content/animations.css",
                "~/shared-directives/box-button/box-button.css",
                "~/shared-directives/auto-input/auto-input.css",
                "~/Content/site.css",
                "~/app/access/access.css"
                ));

            BundleTable.EnableOptimizations = false;
        }
    }
}
