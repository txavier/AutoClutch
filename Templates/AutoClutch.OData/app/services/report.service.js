(function () {
    'use strict';

    angular
        .module('app')
        .factory('reportService', reportService);

    reportService.$inject = ['$http', '$log', '$q'];

    function reportService($http, $log, $q) {
        var apiUrl = 'api/reports';

        var service = {
            getPdfReport: getPdfReport,
            getHtmlReport: getHtmlReport
        };

        return service;

        function getPdfReport(reportServerName, reportPath, reportName, format, parameters) {
            return $http.post(apiUrl + '?reportServerName=' + reportServerName + '&reportPath=' + reportPath + '&reportName=' + reportName + '&format=' + format, parameters, { responseType: 'arraybuffer' })
                        .then(getPdfReportCompleted)
                        .catch(getPdfReportFailed);

            function getPdfReportCompleted(response) {
                return response;
            }

            function getPdfReportFailed(error) {
                $log.error('XHR failed for getActiveInspections. '
                 + (error.data ? error.data.message + ': ' : '') + (error.data ? error.data.message + ': ' + (error.data.messageDetail || error.data.ExceptionMessage || error.data.Message) : ''));

                return error;
            }
        }

        function getHtmlReport(reportServerName, reportPath, reportName, format, parameters) {
            return $http.post(apiUrl + '?reportServerName=' + reportServerName + '&reportPath=' + reportPath + '&reportName=' + reportName + '&format=' + format, parameters)
                        .then(getHtmlReportCompleted)
                        .catch(getHtmlReportFailed);

            function getHtmlReportCompleted(response) {
                return response;
            }

            function getHtmlReportFailed(error) {
                $log.error('XHR failed for getActiveInspections. '
                 + (error.data ? error.data.message + ': ' : '') + (error.data ? error.data.message + ': ' + (error.data.messageDetail || error.data.ExceptionMessage || error.data.Message) : ''));

                return error;
            }
        }

    }
})();
