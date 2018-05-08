(function () {
    'use strict';

    angular
        .module('app')
        .factory('paymentService', paymentService);

    paymentService.$inject = ['dataService'];

    // The purpose of this service is to hold contract information so that the current
    // contract being worked on by the user can have its information displayed on the 
    // sidebar as well as on the current page.  This service is the central place a 
    // loaded contract holds its information.
    function paymentService(dataService) {

        var service = {
            initializeNewPayment: initializeNewPayment,
        };

        return service;

        function initializeNewPayment(contractId) {
            var payment = {};

            return dataService.getInitialPayment(contractId).then(function (data) {
                payment = data;

                payment.contractId = contractId;

                return payment;
            });

        }

    }
})();