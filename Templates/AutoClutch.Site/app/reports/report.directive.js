// Handling the downloading of a pdf: http://stackoverflow.com/questions/21628378/angularjs-display-blob-pdf-in-an-angular-app
(function () {
    'use strict';

    angular
        .module('shared.directives')
        .directive('report', report);

    function report() {
        var directive = {
            restrict: 'EA',
            templateUrl: 'app/reports/report.html',
            scope: {
                format: '@',
                parameters: '=',
                reportServerName: '@',
                reportPath: '@'
            },
            link: link,
            controller: reportController,
            controllerAs: 'vm',
            bindToController: true      // Because the scope is isolated.
        };

        return directive;

        function link(scope, element, attrs, vm) {}
    }

    reportController.$inject = ['$scope', '$log', '$window', '$http', '$sce', '$routeParams', 'reportService'];

    function reportController($scope, $log, $window, $http, $sce, $routeParams, reportService) {
        var vm = this;

        //vm.format = 'HTML4.0';
        vm.format = vm.format || 'pdf';
        vm.openInNewWindow = openInNewWindow;
        vm.getReport = getReport;
        vm.browser = 'unknown';

        activate();

        function activate() {
            vm.format = setFormatForBrowser(vm.format);

            getReport(vm.reportServerName, vm.reportPath, vm.format, vm.parameters);
        }

        $scope.$watch('vm.parameters', function (current, original) {
            if (current !== original) {
                getReport(vm.reportServerName, vm.reportPath, vm.format, vm.parameters);
            }
        });

        Array.prototype.getIndexBy = function (name, value) {
            for (var i = 0; i < this.length; i++) {
                if (this[i][name] == value) {
                    return i;
                }
            }
        }

        function setFormatForBrowser(format) {
            // If the user is using an internet explorer browser
            // then show the HTML report as the default report.
            var browser = getBrowser();

            if (browser == 'ie' || browser == 'edge') {
                return 'HTML4.0';
            }
            else {
                return format;
            }
        }

        function getReport(reportServerName, reportPath, format, parameters) {
            if (format === 'pdf') {
                return reportService.getPdfReport(reportServerName, reportPath, format, parameters).then(function (response) {
                    var file = new Blob([response.data], { type: 'application/pdf' });
                    var fileURL = URL.createObjectURL(file);
                    vm.content = $sce.trustAsResourceUrl(fileURL);
                });
            }
            else if (format === 'excel') {
                return reportService.getPdfReport(reportServerName, reportPath, format, parameters).then(function (response) {
                    var blob = new Blob([response.data], { type: "application/vnd.ms-excel" });
                    var objectUrl = URL.createObjectURL(blob);
                    $window.open(objectUrl);

                    vm.format = setFormatForBrowser('pdf');

                    getReport(vm.reportServerName, vm.reportPath, vm.format, vm.parameters);
                });
            }
            else {
                return reportService.getHtmlReport(reportServerName, reportPath, format, parameters).then(function (response) {
                    vm.content = $sce.trustAsHtml(response.data);
                });
            }

            return vm;
        }

        function getBrowser() {
            var userAgent = $window.navigator.userAgent;

            vm.browser = 'unknown';

            var browsers = { chrome: /chrome/i, safari: /safari/i, firefox: /firefox/i, ie: /msie/i, edge: /trident/i };

            for (var key in browsers) {
                if (browsers[key].test(userAgent)) {
                    vm.browser = key;

                    return vm.browser;
                }
            };

            return vm.browser;
        }

        function openInNewWindow(url) {
            if (url) {
                $window.open(url);
            }
            else {
                $window.open(vm.content);
            }
        }

    }

})();