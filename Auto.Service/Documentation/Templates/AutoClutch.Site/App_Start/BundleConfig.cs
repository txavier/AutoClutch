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
                        "~/wwwroot/lib/jquery/dist/jquery.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/wwwroot/lib/bootstrap/dist/js/bootstrap.min.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                "~/wwwroot/lib/angular/angular.min.js",
                "~/wwwroot/lib/angular-route/angular-route.min.js",
                "~/wwwroot/lib/angular-animate/angular-animate.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/angular-third-party").Include(
                "~/wwwroot/lib/angular-bootstrap/ui-bootstrap-tpls.min.js"
                , "~/wwwroot/lib/nya-bootstrap-select/dist/js/nya-bs-select.min.js"
                , "~/wwwroot/lib/angular-loading-bar/build/loading-bar.min.js"
                , "~/wwwroot/lib/AngularJS-Toaster/toaster.min.js"
                , "~/Scripts/jsnlog.min.js"
                , "~/wwwroot/lib/Chart.js/Chart.js"
                , "~/wwwroot/lib/angular-chart.js/dist/angular-chart.min.js"
                , "~/wwwroot/lib/ng-file-upload/ng-file-upload.min.js"
                , "~/wwwroot/lib/ng-file-upload/ng-file-upload-shim.min.js"
                , "~/wwwroot/lib/angular-bootstrap-datetimepicker/src/js/datetimepicker.js"
                , "~/wwwroot/lib/angular-bootstrap-datetimepicker/src/js/datetimepicker.templates.js"
                , "~/wwwroot/lib/angular-bing-maps/dist/angular-bing-maps.min.js"
                , "~/wwwroot/lib/angular-multi-select/isteven-multi-select.js"
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
                "~/app/services/error.service.js",
                "~/app/services/report.service.js",
                "~/app/services/authorization.service.js",
                "~/app/services/document-upload.service.js",
                "~/app/home/home.controller.js",
                "~/app/sidebar/sidebar.controller.js",
                "~/app/specifications/specification.controller.js",
                "~/app/master-page/master-page.controller.js",
                "~/app/histories/histories.controller.js",
                "~/app/histories/history.controller.js",
                "~/app/payments/payment-pdf.controller.js",
                "~/app/user-action-logs/user-action-logs.controller.js",
                "~/app/users/users.controller.js",
                "~/app/users/update-user.controller.js",
                "~/app/users/add-user.controller.js",
                "~/app/about/about.controller.js"
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
                "~/wwwroot/lib/AngularJS-Toaster/toaster.min.css",
                "~/wwwroot/lib/angular-loading-bar/build/loading-bar.min.css",
                "~/wwwroot/lib/angular-bootstrap/ui-bootstrap-csp.css",
                "~/wwwroot/lib/bootstrap/dist/css/bootstrap.min.css",
                "~/wwwroot/lib/nya-bootstrap-select/dist/css/nya-bs-select.min.css",
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
                "~/app/access/access.css",
                "~/wwwroot/lib/angular-multi-select/isteven-multi-select.css"
                ));

            BundleTable.EnableOptimizations = false;
        }
    }
}
