(function () {
    'use strict';

    angular
        .module('app')
        .controller('AddContractController', AddContractController);

    AddContractController.$inject = ['$scope', '$log', '$window', '$routeParams', '$location', '$filter', 'dataService', 'authenticationService', 'toaster'];

    function AddContractController($scope, $log, $window, $routeParams, $location, $filter, dataService, authenticationService, toaster) {
        var vm = this;

        vm.entityDataStore = 'contracts';
        vm.contract = {};
        vm.engineers = [];
        vm.contractTypes = [];
        vm.history = $window.history;
        vm.addContract = addContract;
        vm.sectionName = '';
        vm.myForm = {};

        activate();

        function activate() {
            vm.sectionName = $routeParams.sectionName;

            getContractTypes();

            getContractStatuses();

            getContractCategories();

            getInitialContract($routeParams.sectionName).then(getEngineers);

            
        }

        function getInitialContract(sectionName) {
            return dataService.getInitialContractOData(sectionName).then(function (data) {
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

                if (vm.engineers.length === 0) {
                    toaster.pop('warning', 'Engineers not in this Section', 'There are no users in this section to assign this contract to.  Please add users to this section.');
                }

                vm.myForm.$setPristine();

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
           
            // If there was no contractNumber specified then use the contractNumberInitialPlaceHolder.
            if (!contract.contractNumber || contract.contractNumber === '') {
                contract.contractNumber = contract.contractNumberInitialPlaceHolder;
            }

            // Check to make sure all of the information we are about to use has been supplied.
            if (!contract.engineerId || !contract.contractTypeId || !contract.contractStatusId || !contract.contractCategoryId) {
                toaster.pop('warning', 'Information Missing', 'Please make sure all of the proper information is set');

                return;
            }

            return dataService.addEntityOData(vm.entityDataStore, contract, true).then(function (data) {
                $location.path('/' + vm.sectionName + '/contracts');
            }, function (error) {
            });

        }
    }

})();