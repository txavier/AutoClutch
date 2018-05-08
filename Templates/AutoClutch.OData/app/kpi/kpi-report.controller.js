(function () {
    'use strict';

    angular
        .module('app')
        .controller('kpiReportController', kpiReportController);

    kpiReportController.$inject = ['$scope', '$routeParams', 'contractService'];

    function kpiReportController($scope, $routeParams, contractService) {
        var vm = this;

        vm.contract = {};

        activate();

        function activate() {
            getContractId($routeParams.contractNumber);
        }

        function getContractId(contractNumber) {
            contractService.setContract(contractNumber).then(function (data) {
                var contract = data;

                vm.contract = contract;
            });
        }

    }

})();