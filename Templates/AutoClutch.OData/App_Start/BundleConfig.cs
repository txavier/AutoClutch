using AspNetBundling;
using System.Web;
using System.Web.Optimization;

namespace $safeprojectname$
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptWithSourceMapBundle("~/bundles/jquery").Include(
                        "~/wwwroot/lib/jquery/jquery.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptWithSourceMapBundle("~/bundles/modernizr").Include(
                        "~/wwwroot/lib/modernizr/modernizr.js"));

            bundles.Add(new ScriptWithSourceMapBundle("~/bundles/bootstrap").Include(
                      "~/wwwroot/lib/bootstrap/dist/js/bootstrap.js",
                      "~/wwwroot/lib/respond.min.js"));

            bundles.Add(new ScriptWithSourceMapBundle("~/bundles/angular").Include(
                "~/wwwroot/lib/angular/angular.min.js"
                , "~/wwwroot/lib/angular-route/angular-route.min.js"
                , "~/wwwroot/lib/angular-animate/angular-animate.min.js"
                , "~/wwwroot/lib/angular-sanitize/angular-sanitize.min.js"
                ));

            bundles.Add(new ScriptWithSourceMapBundle("~/bundles/angular-third-party").Include(
                "~/wwwroot/lib/angular-bootstrap/ui-bootstrap.min.js"
                , "~/wwwroot/lib/angular-bootstrap/ui-bootstrap-tpls.min.js"
                //, "~/wwwroot/lib/nya-bootstrap-select/dist/js/nya-bs-select.js"
                , "~/wwwroot/lib/angular-loading-bar/build/loading-bar.min.js"
                , "~/wwwroot/lib/AngularJS-Toaster/toaster.min.js"
                , "~/wwwroot/lib/stacktrace-js/dist/stacktrace.min.js"
                , "~/Scripts/window-on-error.js"
                , "~/Scripts/jsnlog.min.js"
                , "~/wwwroot/lib/chart.js/dist/Chart.bundle.min.js"
                , "~/wwwroot/lib/angular-chart.js/dist/angular-chart.min.js"
                , "~/wwwroot/lib/ng-file-upload/ng-file-upload.min.js"
                , "~/wwwroot/lib/ng-file-upload/ng-file-upload-shim.min.js"
                , "~/wwwroot/lib/angular-query-builder/angular-query-builder.js"
                , "~/wwwroot/lib/angular-ui-mask/dist/mask.min.js"
                , "~/wwwroot/lib/clipboard/dist/clipboard.min.js"
                , "~/wwwroot/lib/ngclipboard/dist/ngclipboard.min.js"
                , "~/wwwroot/lib/angulartics/dist/angulartics.min.js"
                , "~/wwwroot/lib/angulartics-piwik/src/angulartics-piwik.js"
                ));

            bundles.Add(new ScriptWithSourceMapBundle("~/bundles/angular-third-party-datepicker-moment-shared-directives-moment-filter").Include(
                "~/wwwroot/lib/momentjs/min/moment.min.js"
                , "~/wwwroot/lib/angular-bootstrap-datetimepicker/src/js/datetimepicker.js"
                , "~/shared-directives/moment-filter/moment-filter.js"
                , "~/shared-directives/angular-datetime/datetime.js"
                ));

            bundles.Add(new ScriptWithSourceMapBundle("~/bundles/angular-app").Include(
                "~/app/app.module.js",
                "~/app/app.config.js",
                "~/app/services/data.service.js",
                "~/app/services/odata.service.js",
                "~/app/services/authentication.service.js",
                "~/app/services/contract.service.js",
                "~/app/services/error.service.js",
                "~/app/services/report.service.js",
                "~/app/services/authorization.service.js",
                "~/app/services/document-upload.service.js",
                "~/app/services/payment.service.js",
                "~/app/services/shared.service.js",
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
                "~/app/user-action-logs/user-action-logs.controller.js",
                "~/app/about/about.controller.js",
                "~/app/settings/add-or-update-setting.controller.js",
                "~/app/settings/settings.controller.js",
                "~/app/change-orders/change-order-pdf.controller.js",
                "~/app/work-order-histories/work-order-history-pdf.controller.js",
                "~/app/search/search.controller.js",
                "~/app/change-orders/change-orders.controller.js",
                "~/app/payments/open-payments.controller.js",
                "~/app/locations/location.controller.js",
                "~/app/locations/locations.controller.js",
                "~/app/reporting-categories/reporting-categories.controller.js",
                "~/app/reporting-categories/reporting-category.controller.js",
                "~/app/contract-categories/contract-categories.controller.js",
                "~/app/contract-categories/contract-category.controller.js",
                "~/app/reports/report-viewer.directive.js",
                "~/app/payments/add-receiving-report.directive.js",
                "~/app/payments/update-receiving-report.directive.js",
                "~/app/home/home-procurement.directive.js"
                ));

            bundles.Add(new ScriptWithSourceMapBundle("~/bundles/angular-shared-directives").Include(
                 "~/shared-directives/chart-angular/chartangular.js"
                , "~/shared-directives/autofocus/autofocus.js"
                , "~/shared-directives/shared-directives.js"
                , "~/shared-directives/back-button/back-button.js"
                , "~/shared-directives/confirm-exit/confirm-on-exit.js"
                //,"~/shared-directives/esri-map-geojson/esri-map-geojson.js"
                //, "~/shared-directives/logToServer/logToServer.js"
                , "~/wwwroot/lib/logToServer.js/logToServer.js"
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

            bundles.Add(new ScriptWithSourceMapBundle("~/bundles/sbAdmin").Include(
                "~/Scripts/plugins/morris/raphael-min.js",
                "~/Scripts/plugins/morris/morris.min.js",
                "~/Scripts/plugins/metisMenu/metisMenu.min.js",
                "~/Scripts/sb-admin-2.js"
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/wwwroot/lib/AngularJS-Toaster/toaster.min.css",
                "~/wwwroot/lib/angular-loading-bar/build/loading-bar.min.css",
                "~/wwwroot/lib/angular-bootstrap/ui-bootstrap-csp.css",
                "~/wwwroot/lib/bootstrap/dist/css/bootstrap.min.css",
                "~/wwwroot/lib/nya-bootstrap-select/dist/css/nya-bs-select.min.css",
                "~/Content/plugins/metisMenu/metisMenu.min.css",
                "~/Content/timeline.css",
                "~/Content/sb-admin-2.css",
                "~/Content/plugins/morris.css",
                //"~/fonts/font-awesome-4.6.1/css/font-awesome.min.css",
                "~/wwwroot/lib/angular-bootstrap-datetimepicker/src/css/datetimepicker.css",
                "~/shared-directives/ng-print/ng-print.css",
                "~/wwwroot/lib/angular-chart.js/dist/angular-chart.min.css",
                "~/Content/animations.css",
                "~/shared-directives/box-button/box-button.css",
                "~/shared-directives/auto-input/auto-input.css",
                "~/Content/site.css",
                "~/app/access/access.css"
                ));

            //BundleTable.EnableOptimizations = true;
        }
    }
}