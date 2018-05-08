(function () {
    'use strict';

    angular
        .module('app')
        .controller('PaymentsController', PaymentsController);

    PaymentsController.$inject = ['$scope', '$log', '$routeParams', '$location', 'dataService', 'contractService', 'authenticationService'];

    function PaymentsController($scope, $log, $routeParams, $location, dataService, contractService, authenticationService) {
        var vm = this;

        vm.entityDataStore = 'payments';
        vm.entities = [];
        vm.searchEntities = searchEntities;
        vm.deleteEntity = deleteEntity;
        vm.softDeleteEntity = softDeleteEntity;
        vm.totalItems = 0;
        vm.itemsPerPage = 1000;
        vm.currentPage = 1;
        vm.pageChanged = pageChanged;
        vm.setSortOrder = setSortOrder;
        vm.orderBy = 'paymentNumber desc';
        vm.searchText = null;
        vm.searchCriteria = {};
        vm.searchCriteria.searchText = null;
        vm.includeProperties = 'paymentType,contract($expand=payments)';
        vm.q = null;
        vm.contract = {};
        vm.expandContractDetails = expandContractDetails;
        vm.openContractDetails = false;
        vm.disableAll = false;
        vm.loggedInUser = {};

        activate();

        function activate() {
            getLoggedInUser();

            vm.contract.contractNumber = $routeParams.contractNumber;

            getPayments(vm.contract.contractNumber);

            contractService.setContract(vm.contract.contractNumber).then(setViewDisabledBoxes);

            return vm;
        }

        function getLoggedInUser() {
            return authenticationService.getLoggedInUser().then(function (data) {
                vm.loggedInUser = data;

                return vm.loggedInUser;
            });
        }

        function setViewDisabledBoxes(contract) {
            // The contract status 'Closed' is contract status ID 5.
            // The contract status 'Cancelled' is contract status ID 7.
            // If this is a closed or cancelled contract then we are not allowing the 
            // modification of any of the elements.
            if (contract.contractStatusId == 5 || contract.contractStatusId == 7) {
                vm.disableAll = true;
            }

            return vm.disableAll;
        }

        function expandContractDetails() {
            vm.openContractDetails = !vm.openContractDetails;
        }

        function getPayments(contractNumber) {
            return dataService.getContractIdOData(contractNumber).then(function (data) {
                vm.q = 'contractId=' + data;

                setSearchCriteria(vm.currentPage, vm.itemsPerPage, vm.orderBy, vm.searchText, vm.includeProperties, vm.q);

                searchEntities(vm.searchCriteria);

                searchEntitiesCount(vm.searchCriteria);
            });
        }

        function setSortOrder(orderBy) {
            if (vm.orderBy.indexOf(' desc') === -1) {
                vm.orderBy = vm.orderBy + ' desc';
            } else {
                vm.orderBy = orderBy.replace('desc', '');
            }

            setSearchCriteria(vm.currentPage, vm.itemsPerPage, vm.orderBy, vm.searchText, vm.includeProperties, vm.q);

            searchEntities(vm.searchCriteria);
        }

        function searchEntities(searchCriteria) {
            return dataService.searchEntitiesOData(vm.entityDataStore, searchCriteria).then(function (data) {
                vm.entities = data;

                return vm.entities;
            });
        }

        function searchEntitiesCount(searchCriteria) {
            return dataService.searchEntitiesCountOData(vm.entityDataStore, searchCriteria).then(function (data) {
                vm.totalItems = data || 0;

                return vm.totalItems;
            });
        }

        function setSearchCriteria(currentPage, itemsPerPage, orderBy, searchText, includeProperties, q) {
            vm.searchCriteria = {
                currentPage: currentPage,
                itemsPerPage: itemsPerPage,
                orderBy: orderBy,
                searchText: searchText,
                includeProperties: includeProperties,
                q: q
            }

            return vm.searchCriteria;
        }

        function softDeleteEntity(id, entityDataStore) {
            return dataService.softDeleteEntity(entityDataStore || vm.entityDataStore, id)
                .then(function (data) {
                    activate();
                });
        }

        function deleteEntity(id, entityDataStore) {
            return dataService.deleteEntity(entityDataStore || vm.entityDataStore, id)
                .then(function (data) {
                    activate();
                });
        }

        function pageChanged() {
            setSearchCriteria(vm.currentPage, vm.itemsPerPage, vm.orderBy, vm.searchText, vm.includeProperties, vm.q);

            searchEntities(vm.searchCriteria);
        }

        $scope.$on('$locationChangeStart', function (event) {
            if ($location.path().split('contracts/').length >= 2
                && $location.path().split('contracts/')[1].split('/').length > 0) {

                // If the contract number is not being changed in the url, meaning the user is on
                // the same contract just a different view then do not reset the contract information
                // in the contract service.  The sidebar will need to continue to use this information
                // to keep the sidebar open and reflecting accurate information, i.e. Work Orders (33).
                if (vm.contract.contractNumber == $location.path().split('contracts/')[1].split('/')[0]) {
                    return;
                }
            }

            // Reset the contract service contract information for resetting the 
            // 'pushOut' sidebar.
            contractService.setContractId(0);

            contractService.setContractWorkOrders([]);

            contractService.setContractNumber(null);
        });
    }

})();