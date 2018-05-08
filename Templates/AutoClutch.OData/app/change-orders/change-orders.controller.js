(function () {
    'use strict';

    angular
        .module('app')
        .controller('ChangeOrdersController', ChangeOrdersController);

    ChangeOrdersController.$inject = ['$scope', '$log', '$routeParams', '$filter', 'dataService', 'authenticationService'];

    function ChangeOrdersController($scope, $log, $routeParams, $filter, dataService, authenticationService) {
        var vm = this;

        vm.entityDataStore = 'changeOrders';
        vm.entities = [];
        vm.softDeleteEntity = softDeleteEntity;
        vm.totalItems = 0;
        vm.itemsPerPage = 10;
        vm.currentPage = 1;
        vm.pageChanged = pageChanged;
        vm.setSortOrder = setSortOrder;
        vm.orderBy = 'contract/contractNumber,changeOrderNumber desc';
        vm.searchText = null;
        vm.searchCriteria = {};
        vm.searchCriteria.searchText = null;
        vm.query = '';

        activate();

        function activate() {
            vm.contractNumber = $routeParams.contractNumber;

            setQuery(vm.contractNumber);

            getLoggedInUser().then(function (data) {
                vm.loggedInUser = data;

                setSearchCriteria(vm.currentPage, vm.itemsPerPage, vm.orderBy, vm.searchText, vm.query);

                searchEntities(vm.entityDataStore, vm.searchCriteria);

                searchEntitiesCount(vm.entityDataStore, vm.searchCriteria);
            });

            return vm;
        }
        
        function getLoggedInUser() {
            return authenticationService.getLoggedInUser().then(function (data) {
                vm.loggedInUser = data;

                return vm.loggedInUser;
            });
        }

        function setQuery(contractNumber) {
            vm.query = 'contract/contractNumber="' + contractNumber + '"';
        }

        function setSortOrder(orderBy) {
            if (vm.orderBy.indexOf(' desc') === -1) {
                vm.orderBy = vm.orderBy + ' desc';
            } else {
                vm.orderBy = orderBy.replace('desc', '');
            }

            setSearchCriteria(vm.currentPage, vm.itemsPerPage, vm.orderBy, vm.searchText, vm.query);

            searchEntities(vm.entityDataStore, vm.searchCriteria);
        }

        function searchEntities(entityDataStore, searchCriteria) {
            return dataService.searchEntitiesOData(entityDataStore, searchCriteria).then(function (data) {
                vm.entities = data;

                return vm.entities;
            });
        }

        function searchEntitiesCount(entityDataStore, searchCriteria) {
            return dataService.searchEntitiesCountOData(entityDataStore, searchCriteria).then(function (data) {
                vm.totalItems = data || 0;

                return vm.totalItems;
            });
        }

        function setSearchCriteria(currentPage, itemsPerPage, orderBy, searchText, query) {
            vm.searchCriteria = {
                currentPage: currentPage,
                itemsPerPage: itemsPerPage,
                orderBy: orderBy,
                searchText: searchText,
                includeProperties: 'contract($expand=section),changeOrderType',
                q: query
            }

            if (!vm.contractNumber) {
                setSearchCriteriaForAllWorkOrders();
            }

            return vm.searchCriteria;
        }

        function setSearchCriteriaForAllWorkOrders() {
            vm.searchCriteria.q = 'registered=null';

            // If this user is in the database.
            if (vm.loggedInUser) {
                vm.searchCriteria.q += ' and contract/sectionId=' + vm.loggedInUser.sectionId;
            }

            if (!isTopLevelUser() && vm.loggedInUser) {
                vm.searchCriteria.q += ' and contract/engineerId=' + vm.loggedInUser.engineerId;
            }
        }

        function isTopLevelUser() {
            if (!vm.loggedInUser) {
                return false;
            }

            var result = vm.loggedInUser.sectionChiefRole || vm.loggedInUser.adminRole || vm.loggedInUser.areaEngineerRole;

            return result;
        }
        function softDeleteEntity(id, entityDataStore) {
            return dataService.softDeleteEntity(entityDataStore || vm.entityDataStore, id)
                .then(function (data) {
                    activate();
                });
        }


        function pageChanged() {
            setSearchCriteria(vm.currentPage, vm.itemsPerPage, vm.orderBy, vm.searchText, vm.query);

            searchEntities(vm.entityDataStore, vm.searchCriteria);
        }
    }

})();