(function () {
    'use strict';

    angular
        .module('app')
        .controller('ContractController', ContractController);

    ContractController.$inject = ['$scope', '$routeParams'];

    function ContractController($scope, $routeParams) {
        var vm = this;

        vm.entityDataStore = 'contracts';
        vm.contract = {};

        activate();

        function activate(cache) {
            vm.contract = {};

            vm.contract.contractNumber = $routeParams.contractNumber;
        }

    }

})();