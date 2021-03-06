﻿(function () {
    'use strict';

    angular
        .module('app')
        .controller('PaymentPdfController', PaymentPdfController);

    PaymentPdfController.$inject = ['$scope', '$log', '$window', '$http', '$sce', '$routeParams', 'dataService', 'toaster', 'authenticationService'];

    function PaymentPdfController($scope, $log, $window, $http, $sce, $routeParams, dataService, toaster, authenticationService) {
        var vm = this;

        vm.payment = {};
        vm.openInNewWindow = openInNewWindow;
        vm.content = '';

        activate();

        function activate() {
            getPayment($routeParams.paymentId);
        }

        function getPayment(id) {
            return dataService.getEntity('payments', id).then(function (data) {
                vm.payment = data;

                getFileByteStream(vm.payment.paymentPDF, vm.payment.paymentPDFFileId);
                
                return vm.payment;
            });
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

        function getFileByteStream(address, fileId) {
            return dataService.getFileByteStream(address, fileId).then(function (response) {
                var file = new Blob([response.data], { type: 'application/pdf' });
                var fileURL = URL.createObjectURL(file);
                vm.content = $sce.trustAsResourceUrl(fileURL);
            });

            return vm;
        }

        function getBrowser() {
            var userAgent = $window.navigator.userAgent;

            var browsers = { chrome: /chrome/i, safari: /safari/i, firefox: /firefox/i, ie: /msie/i, edge: /trident/i };

            for (var key in browsers) {
                if (browsers[key].test(userAgent)) {
                    return key;
                }
            };

            return 'unknown';
        }

        function openInNewWindow() {
            $window.open(vm.content);
        }

    }

})();