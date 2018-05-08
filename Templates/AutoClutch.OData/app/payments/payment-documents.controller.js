(function () {
    'use strict';

    angular
        .module('app')
        .controller('PaymentDocumentsController', PaymentDocumentsController);

    PaymentDocumentsController.$inject = ['$scope', '$log', '$routeParams', 'dataService', 'authenticationService', 'contractService'];

    function PaymentDocumentsController($scope, $log, $routeParams, dataService, authenticationService, contractService) {
        var vm = this;

        vm.contractId = null;
        vm.paymentNumber = null;
        vm.paymentId = null;
        vm.payrollQuestion = false;
        vm.minimumQuestion = false;
        vm.signInQuestion = false;
        vm.copyQuestion = false;
        vm.PrevailingSubcontractors = 'None';
        vm.nonPrevailingSubcontractors = 'None';
        vm.reason = '';
        vm.getParameters = getParameters;
        vm.contract = {};

        activate();

        function activate() {
            setRoutingValues($routeParams.contractNumber, $routeParams.paymentId);

            getPayment($routeParams.paymentId);
        }

        function getParameters(reportType) {
            var parameters = {};

            parameters['ContractId'] = vm.contractId || 30;

            parameters['PaymentNumber'] = vm.paymentNumber || 286;

            parameters['nonPrevailingSubcontractors'] = vm.nonPrevailingSubcontractors;

            parameters['PrevailingSubcontractors'] = vm.PrevailingSubcontractors;

            parameters['Question1'] = vm.payrollQuestion;

            parameters['Question2'] = vm.minimumQuestion;

            parameters['Question3'] = vm.signInQuestion;

            parameters['Question4'] = vm.copyQuestion;

            if (reportType === 'form2') {
                parameters['Reason'] = vm.reason || '';
            }

            return parameters;
        }

        function setRoutingValues(contractNumber, paymentId) {
            vm.paymentId = paymentId;

            var contractSearchCriteria = {
                q: 'contractNumber = "' + contractNumber + '"'
            };

            return dataService.searchEntities('contracts', contractSearchCriteria).then(function (data) {
                vm.contract = data[0];

                vm.contractId = vm.contract.contractId;

                return vm;
            });
        }

        function getContract(searchCriteria) {
            return dataService.searchCriteria('contracts', searchCriteria).then(function (data) {
                vm.contract = data;

                return vm.contract;
            });
        }

        function getPayment(id) {
            return dataService.getEntity('payments', id).then(function (data) {
                vm.paymentNumber = data.paymentNumber;

                return vm.paymentNumber;
            });
        }

    }

})();