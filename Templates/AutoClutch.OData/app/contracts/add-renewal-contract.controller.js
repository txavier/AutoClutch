(function () {
    'use strict';

    angular
        .module('app')
        .controller('AddRenewalContractController', AddRenewalContractController);

    AddRenewalContractController.$inject = ['$scope', '$log', '$window', '$routeParams', '$filter', '$location', 'dataService', 'authenticationService', 'toaster'];

    function AddRenewalContractController($scope, $log, $window, $routeParams, $filter, $location, dataService, authenticationService, toaster) {
        var vm = this;

        vm.entityDataStore = 'contracts';
        vm.contract = {};
        vm.engineers = [];
        vm.contractTypes = [];
        vm.history = $window.history;
        vm.addContract = addContract;
        vm.myForm = {};
        vm.sectionName = '';

        activate();

        function activate() {
            vm.sectionName = $routeParams.sectionName;

            getContractTypes();

            getContractStatuses();

            getContractCategories();

            getInitialRenewalContract($routeParams.contractNumber).then(getEngineers);
        }

        function getInitialRenewalContract(originalContractNumber) {
            return dataService.getInitialRenewalContract(originalContractNumber).then(function (data) {
                vm.contract = data;

                return vm.contract;
            });
        }

        function getEngineers() {
            return dataService.searchEntitiesOData('engineers').then(function (data) {
                var orderedData = $filter('orderBy')(data, 'name');

                // Code to filter a list in a controller 
                // http://voryx.net/using-angularjs-filter-inside-the-controller/
                var filteredData = $filter('filter')(orderedData, { sectionId: vm.contract.sectionId });

                vm.engineers = filteredData;

                return vm.engineers;
            });
        }

        function getContractTypes() {
            return dataService.searchEntitiesOData('contractTypes', { orderBy: 'name' }).then(function (data) {
                vm.contractTypes = data;

                return vm.contractTypes;
            });
        }

        function getContractStatuses() {
            return dataService.searchEntitiesOData('contractStatuses').then(function (data) {
                vm.contractStatuses = data;

                return vm.contractStatuses;
            });
        }

        function getContractCategories() {
            return dataService.searchEntitiesOData('contractCategories', { orderBy: 'name' }).then(function (data) {
                vm.contractCategories = data;

                return vm.contractCategories;
            });
        }

        function addContract(contract) {
            if (!vm.myForm.$valid) {
                var errors = [];

                for (var key in vm.myForm.$error) {
                    for (var index = 0; index < vm.myForm.$error[key].length; index++) {
                        errors.push(vm.myForm.$error[key][index].$name + ' is required.');
                    }
                }

                toaster.pop('warning', 'Information Missing', 'The ' + errors[0]);

                return;
            }

            //contract.engineerId = contract.engineer.engineerId;

            //contract.engineer = null;

            contract.currentEngineerIdDisplay = null;

            return dataService.addRenewalContract(vm.entityDataStore, contract, true).then(function (data) {
                $location.path('/' + vm.sectionName + '/contracts/' + data.contractNumber);
            }, function (error) {
            });
        }
    }

})();